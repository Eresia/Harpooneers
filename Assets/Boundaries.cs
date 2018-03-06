using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour {

    public Vector3 size;

    public void UpdateBoundaries(Vector3 newSize)
    {
        // Lerp size or apply directly new size ?
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
