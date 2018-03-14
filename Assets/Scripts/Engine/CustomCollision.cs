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
        bool bump = true;
        
        // Don't kill the player when hit a player
        if (collision.gameObject.CompareTag("Player"))
        {
			GameManager.instance.audioManager.PlaySoundOneTime(collision_sound, 0.05f);
            CustomCollision otherPlayer = collision.gameObject.GetComponent<CustomCollision>();
            if(!playerMgr.IsDead && otherPlayer.playerMgr.IsDead)
            {
                bump = false;
                otherPlayer.playerMgr.Resurrect();
            }

			killPlayer = false;
        }

        // or a floating object.
        if (collision.gameObject.CompareTag("FloatingObject"))
        {
            killPlayer = false;
        }

        // Kill player if the ship moves too fast.
        if (killPlayer && physic.Velocity.sqrMagnitude > killingForce)
        {
            playerMgr.Death();
        }

        // Remove player velocity in any case.
        physic.AddForce(-physic.Velocity);

        if(bump)
        {
            physic.AddForce(collision.contacts[0].normal * bumpForce);
        }
    }
}
