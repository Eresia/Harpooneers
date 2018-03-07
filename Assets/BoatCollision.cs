using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatCollision : MonoBehaviour {

    public CapsuleCollider shipCollider;
    public PhysicMove physic;
    
    public float rayLength = 5f;
    public LayerMask blockingLayer;

    [Header("Death when collide")]
    public PlayerManager playerMgr;
    public float killingForce = 150f;

    public float bumpForce = 15f;

    private RaycastHit hit;

	// Update is called once per frame
	void Update () {

        Vector3 shipCenter = transform.TransformPoint(shipCollider.center);
        Ray r = new Ray(shipCenter, physic.velocity.normalized);

        Debug.DrawRay(r.origin, r.direction * rayLength, Color.red, 1f);

        if(Physics.Raycast(r, out hit, rayLength, blockingLayer))
        {
            // Ignore itself.
            if(hit.collider == shipCollider)
            {
                return;
            }

            Vector3 currentVelocity = physic.velocity;
            bool hitPlayer = false;

            // Don't kill the player when hit a player.
            if (hit.collider.CompareTag("Player"))
            {
                hitPlayer = true;
            }

            Debug.Log("Impact force : " + Mathf.RoundToInt(currentVelocity.sqrMagnitude));

            // Kill player if the ship moves too fast.
            if (currentVelocity.sqrMagnitude > killingForce && !hitPlayer)
            {
                playerMgr.Death();
            }

            // Create bump
            physic.AddForce(-currentVelocity * bumpForce);
        }
	}
}
