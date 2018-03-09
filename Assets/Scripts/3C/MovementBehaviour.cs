using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicMove))]
public class MovementBehaviour : MonoBehaviour {

    public CoqueModule coqueModule;

    // Lerp progressif sur la direction du bateau. (Direction desiree (INPUT) et direction actuelle.

    private Quaternion initialDir;
    private Quaternion targetDir;

	public PhysicMove physicMove;

    private float move;

    private void Reset()
    {
        physicMove = GetComponent<PhysicMove>();
    }

    private void Awake()
    {
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
            transform.rotation = Quaternion.Lerp(initialDir, targetDir, Time.deltaTime * coqueModule.turnSpeed);
        }

        // Move boat toward.
        physicMove.AddForce(transform.forward * coqueModule.moveSpeed * move);

        // Limit position in the boundaries of the screen.
        Vector3 pos = transform.position;

        //Vector3 hitPoint = GameManager.instance.boundaryMgr.InScreenPosition(pos);

        //pos.x = GameManager.instance.boundaryMgr.;
        //pos.y = 
        //pos.z = hitPoint.z;

        transform.position = GameManager.instance.boundaryMgr.InScreenPosition(pos);
    }

    // Freeze player at his position.
    public void FreezePlayer()
    {
        move = 0f;
    }
}
