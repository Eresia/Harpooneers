using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicMove))]
public class MovementBehaviourNewPhysic : MonoBehaviour {

    public float moveSpeed = 5f;
    public float maxSpeed = 10f;
    public float rotationSpeed;

    [Header("Metrics for boundaries")]
    [Tooltip("Distance with boundaries.")]
    public float offsetX = 1f;
    public float offsetZ = 1f;

    // Lerp progressif sur la direction du bateau. (Direction desiree (INPUT) et direction actuelle.

    private Quaternion initialDir;
    private Quaternion targetDir;

	private PhysicMove physicMove;

    private float move;

    private void Awake()
    {
		physicMove = GetComponent<PhysicMove>();
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
        physicMove.AddForce(transform.forward * moveSpeed * move);
    }
}
