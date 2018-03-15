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

    public float height;
}

public class BoundaryManager : MonoBehaviour {

    public Camera cam;
    public LayerMask seaMask;

    public float horizontalOffset = 0.1f;
    public float verticalOffset = 0.1f;

    public float resetHorizontalOffset = 0.1f;
    public float resetVerticalOffset = 0.1f;

    public float forceWhenKillByBOundaries = 40f;

    private Vector3[] limits;

    [HideInInspector]
    public BoundariesData trapezeData;

    public bool debug;

    [HideInInspector]
    public Vector3 screenCenterInWorldSpace;

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

        // Get center of the screen in world space.
        if (Physics.Raycast(cam.ViewportPointToRay(new Vector3(0.5f, 0.5f)), out hit, 10000f, seaMask))
        {
            screenCenterInWorldSpace = hit.point;
        }

        // Update trapeze data
        trapezeData.zBottom = limits[0].z;
        trapezeData.zTop = limits[1].z;

        trapezeData.xBottomLeft = limits[0].x;
        trapezeData.xTopLeft = limits[1].x;

        trapezeData.xBottomRight = limits[3].x;
        trapezeData.xTopRight = limits[2].x;

        trapezeData.height = Mathf.Abs(trapezeData.zBottom - trapezeData.zTop);
    }

    /// <summary>
    /// Return position of a boat inside the screen.
    /// </summary>
    /// <returns></returns>
    public Vector3 LimitPosition(Vector3 boatPos)
    {
        Vector3 clampedPos = boatPos;
        
        float t = Mathf.Abs(trapezeData.zBottom - boatPos.z) / trapezeData.height;

        float xMin = Mathf.Lerp(trapezeData.xBottomLeft, trapezeData.xTopLeft, t);
        float xMax = Mathf.Lerp(trapezeData.xBottomRight, trapezeData.xTopRight, t);
        
        clampedPos.x = Mathf.Clamp(clampedPos.x, xMin - xMin * horizontalOffset, xMax - xMax * horizontalOffset);
        clampedPos.z = Mathf.Clamp(clampedPos.z, trapezeData.zBottom - trapezeData.zBottom * verticalOffset, trapezeData.zTop - trapezeData.zTop * verticalOffset);

        return clampedPos;
    }

    /// <summary>
    /// Return 0 if pos is Ok, 1 if player is out of screen, 2 if player is too far.
    /// </summary>
    /// <returns></returns>
    public int CheckPos(Vector3 boatPos)
    {
        float t = Mathf.Abs(trapezeData.zBottom - boatPos.z) / trapezeData.height;

        float zMin = trapezeData.zBottom;
        float zMax = trapezeData.zTop;
        float xMin = Mathf.Lerp(trapezeData.xBottomLeft, trapezeData.xTopLeft, t);
        float xMax = Mathf.Lerp(trapezeData.xBottomRight, trapezeData.xTopRight, t);

        if (boatPos.x < xMin || boatPos.x > xMax)
        {
            if (boatPos.x < xMin + xMin * verticalOffset || boatPos.x > xMax + xMax * verticalOffset)
            {
                return 2;
            }

            return 1;
        }

        else if (boatPos.z < zMin || boatPos.z > zMax)
        {
            float offset = zMin * verticalOffset;

            if (boatPos.z < zMin + zMin * offset || boatPos.z > zMax + zMax * offset)
            {
                return 2;
            }

            return 1;
        }

        return 0;
    }

    public bool IsInScreen(Vector3 pos)
    {
        Vector3 posInScreen = Camera.main.WorldToViewportPoint(pos);
        
        if(posInScreen.x < 0f - resetHorizontalOffset || posInScreen.x > 1f + resetHorizontalOffset)
        {
            return true;
        }

        if (posInScreen.y < 0f - resetVerticalOffset || posInScreen.y > 1f + resetVerticalOffset)
        {
            return true;
        }

        return false;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

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

    public Vector3 ScreenToWorldPos(Vector2 screenPos)
    {
        Vector3 worldPos = Vector3.zero;

        worldPos.z = Mathf.Lerp(trapezeData.zBottom, trapezeData.zTop, screenPos.y);

        float xMin = Mathf.Lerp(trapezeData.xBottomLeft, trapezeData.xTopLeft, screenPos.y);
        float xMax = Mathf.Lerp(trapezeData.xBottomRight, trapezeData.xTopRight, screenPos.y);

        worldPos.x = Mathf.Lerp(xMin, xMax, screenPos.x);
        
        return worldPos;
    }
}
