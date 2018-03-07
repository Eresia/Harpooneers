using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactBehaviour : MonoBehaviour {

    public PlayerManager playerMgr;

    public float destructionForce = 9f;

    private void Reset()
    {
        playerMgr = GetComponent<PlayerManager>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Don't kill the player when hit a player.
        if(collision.collider.CompareTag("Player"))
        {
            return;
        }

        Debug.Log("Impact force : " + collision.relativeVelocity.magnitude);

        // Kill the player if the force of impact is too big.
        if(collision.relativeVelocity.magnitude > destructionForce)
        {
            playerMgr.Death();
        }
    }
}
