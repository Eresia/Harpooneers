using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour {

    public float moveSpeed;
    public float rotationSpeed;

    // Lerp progressif sur la direction du bateau. (Direction desiree (INPUT) et direction actuelle.

    public Vector3 inputDir;
    public Vector3 currentDir;

    private Rigidbody rgbd;

    private void Awake()
    {
        
    }

    public void Move(Vector3 inputDir) {
        
        if(currentDir != inputDir)
        {
            currentDir = Vector3.Lerp(inputDir, currentDir, rotationSpeed * Time.deltaTime);
        }

        rgbd.MoveRotation(Quaternion.Euler(currentDir));

        rgbd.AddForce(transform.forward * moveSpeed, ForceMode.Impulse);

        //rgbd.AddForce(currentDir * moveSpeed, ForceMode.Acceleration);
    }
}
