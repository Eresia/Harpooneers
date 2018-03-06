using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour {

    public Camera cam;
    public LayerMask seaMask;

    public float horizontalOffset = 0.1f;
    public float verticalOffset = 0.1f;

    private Vector3[] limits;

    private void Awake()
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
    }

    /// <summary>
    /// Return position of a bot inside the screen.
    /// </summary>
    /// <returns></returns>
    public Vector3 InScreenPosition(Vector3 boatPos)
    {
        Vector3 posInScreen = Camera.main.WorldToViewportPoint(boatPos);

        posInScreen.x = Mathf.Clamp(posInScreen.x, 0f + horizontalOffset, 1f - horizontalOffset);
        posInScreen.y = Mathf.Clamp(posInScreen.y, 0f + verticalOffset, 1f - verticalOffset);
        posInScreen.z = 0f;

        Ray r = Camera.main.ViewportPointToRay(posInScreen);

        RaycastHit hit;
        if (Physics.Raycast(r, out hit, 10000f, seaMask))
        {
            Debug.DrawRay(r.origin, r.direction * hit.distance, Color.green);
        }

        return hit.point;
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
