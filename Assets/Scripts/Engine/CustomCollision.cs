using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCollision : MonoBehaviour {
    
    public PhysicMove physic;
    
    public float rayLengthForward = 5f;
    public float rayLengthLeftRight = 2f;
    public LayerMask blockingLayer;

    [Header("Death when collide")]
    public PlayerManager playerMgr;
    public float killingForce = 150f;

    public float bumpForce = 15f;

	public AudioClip collision_sound;
    
    protected void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        HandleCollision(collision);
    }

    void HandleCollision(Collision collision)
    {
        //Debug.Log("Impact force : " + Mathf.RoundToInt(physic.Velocity.sqrMagnitude));
        


        bool killPlayer = true;

        // Don't kill the player when hit a player or a floating object.
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("FloatingObject"))
        {
			GameManager.instance.audioManager.PlaySoundOneTime (collision_sound);
			killPlayer = false;
        }

        // Kill player if the ship moves too fast.
        if (physic.Velocity.sqrMagnitude > killingForce && killPlayer)
        {
            playerMgr.Death();
        }

        // Bump the player in any case.
        physic.AddForce(-physic.Velocity);
        physic.AddForce(collision.contacts[0].normal * bumpForce);
    }

    /*
    // Update is called once per frame
    void FixedUpdate () {

        Vector3 shipCenter = transform.TransformPoint(shipCollider.center);
        Ray r = new Ray(shipCenter, transform.forward);
        Ray rRight = new Ray(shipCenter, transform.right);
        Ray rLeft = new Ray(shipCenter, -transform.right);

        Debug.DrawRay(r.origin, r.direction * rayLengthForward, Color.red, 1f);
        Debug.DrawRay(rRight.origin, rRight.direction * rayLengthLeftRight, Color.green, 1f);
        Debug.DrawRay(rLeft.origin, rLeft.direction * rayLengthLeftRight, Color.magenta, 1f);

        HandleRay(r, rayLengthForward);
        HandleRay(rLeft, rayLengthLeftRight);
        HandleRay(rRight, rayLengthLeftRight);
    }

    private void HandleRay(Ray r, float rayLength)
    {
        if (Physics.Raycast(r, out hit, rayLength, blockingLayer))
        {
            Vector3 currentVelocity = physic.velocity;
            Debug.Log("Impact force : " + Mathf.RoundToInt(currentVelocity.sqrMagnitude));
            
            // Create bump
            physic.AddForce(-currentVelocity * bumpForce);
        }
    }
    */
}
