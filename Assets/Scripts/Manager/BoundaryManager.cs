using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BoundariesData
{
    public float xBottomLeft;
    public float xBottomRight;
    public float xTopLeft;
    public float xTopRight;
    public float zBottom;
    public float zTop;

    public float sideLength;
}

public class BoundaryManager : MonoBehaviour {

    public Camera cam;
    public LayerMask seaMask;

    public float horizontalOffset = 0.1f;
    public float verticalOffset = 0.1f;
    
    private Vector3[] limits;

    public BoundariesData trapezeData;

    private void Awake()
    {
        UpdateBoundaries();
    }

    public void UpdateBoundaries()
    {
        // Calculate new boundaries.
        limits = new Vector3[4];

        RaycastHit hit;

        if (Physics.Raycast(cam.ViewportPointToRay(new Vector3(0, 0)), out hit, 10000f, seaMask))
        {
            limits[0] = hit.point;
        }

        if (Physics.Raycast(cam.ViewportPointToRay(new Vector3(0, 1)), out hit, 10000f, seaMask))
        {
            limits[1] = hit.point;
        }

        if (Physics.Raycast(cam.ViewportPointToRay(new Vector3(1, 1)), out hit, 10000f, seaMask))
        {
            limits[2] = hit.point;
        }

        if (Physics.Raycast(cam.ViewportPointToRay(new Vector3(1, 0)), out hit, 10000f, seaMask))
        {
            limits[3] = hit.point;
        }

        // Update trapeze data
        trapezeData.zBottom = limits[0].z;
        trapezeData.zTop = limits[1].z;

        trapezeData.xBottomLeft = limits[0].x;
        trapezeData.xTopLeft = limits[1].x;

        trapezeData.xBottomRight = limits[3].x;
        trapezeData.xTopRight = limits[2].x;

        trapezeData.sideLength = Mathf.Abs(trapezeData.zBottom - trapezeData.zTop);
    }

    /// <summary>
    /// Return position of a boat inside the screen.
    /// </summary>
    /// <returns></returns>
    public Vector3 InScreenPosition(Vector3 boatPos)
    {
        Vector3 clampedPos = boatPos;
        
        float t = Mathf.Abs(trapezeData.zBottom - boatPos.z) / trapezeData.sideLength;

        float xMin = Mathf.Lerp(trapezeData.xBottomLeft, trapezeData.xTopLeft, t);
        float xMax = Mathf.Lerp(trapezeData.xBottomRight, trapezeData.xTopRight, t);

        //Debug.Log(t + ": " + xMin + " " + xMax);

        clampedPos.x = Mathf.Clamp(clampedPos.x, xMin + horizontalOffset, xMax - horizontalOffset);
        clampedPos.z = Mathf.Clamp(clampedPos.z, trapezeData.zBottom + verticalOffset, trapezeData.zTop - verticalOffset);

        return clampedPos;
    }

    public bool IsInScreen(Vector3 pos)
    {
        Vector3 posInScreen = Camera.main.WorldToViewportPoint(pos);
        
        if(posInScreen.x < 0f || posInScreen.x > 1f)
        {
            return true;
        }

        if (posInScreen.y < 0f || posInScreen.y > 1f)
        {
            return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        if(limits != null && limits.Length > 0)
        {
            foreach(Vector3 p in limits)
            {
                Gizmos.DrawSphere(p, 0.25f);
            }

            for (int i = 0; i < limits.Length; i++)
            {
                Gizmos.DrawLine(limits[i], limits[(i+1) % 4]);
            }
        }
    }
}
