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
	public AudioClip death_sound;
    
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
        
        // Don't kill the player when hit a player
        if (collision.gameObject.CompareTag("Player"))
        {
            CustomCollision otherPlayer = collision.gameObject.GetComponent<CustomCollision>();
            if(!playerMgr.IsDead && otherPlayer.playerMgr.IsDead)
            {
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
        if (physic.Velocity.sqrMagnitude > killingForce && killPlayer)
        {
            playerMgr.Death();
        }

        GameManager.instance.audioManager.PlaySoundOneTime(collision_sound, 0.05f);
        // Bump the player in any case.
        physic.AddForce(-physic.Velocity);
        physic.AddForce(collision.contacts[0].normal * bumpForce);
    }
}
