using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactBehaviour : MonoBehaviour {

    public PlayerManager playerMgr;

    public float destructionForce = 9f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Impact force : " + collision.relativeVelocity.magnitude);

        // Kill the player if the force of impact is too big.
        if(collision.relativeVelocity.magnitude > destructionForce)
        {
            playerMgr.Death();
        }
    }
}
