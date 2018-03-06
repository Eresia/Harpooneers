using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBehaviour : MonoBehaviour {

    public float moveSpeed = 5f;
    public float maxSpeed = 10f;
    public float rotationSpeed;

    // Lerp progressif sur la direction du bateau. (Direction desiree (INPUT) et direction actuelle.

    private Quaternion initialDir;
    private Quaternion targetDir;

    private Rigidbody rgbd;

    private float move;

    private void Awake()
    {
        rgbd = GetComponent<Rigidbody>();

        initialDir = targetDir = Quaternion.identity;
    }

    public void Move(Vector3 inputDir) {

        if(inputDir == Vector3.zero)
        {
            move = 0f;

            return;
        }

        move = 1f;

        initialDir = transform.rotation;
        targetDir = Quaternion.LookRotation(inputDir);
    }

    private void FixedUpdate()
    {
        if(initialDir != targetDir)
        {
            // Turn boat.
            transform.rotation = Quaternion.Lerp(initialDir, targetDir, Time.deltaTime * rotationSpeed);
        }

        // Move boat toward.
        rgbd.AddForce(transform.forward * moveSpeed * move, ForceMode.Force);

        // Limit max speed.
        rgbd.velocity = Vector3.ClampMagnitude(rgbd.velocity, maxSpeed);


        //transform.position = Mathf.Clamp()
    }
}
