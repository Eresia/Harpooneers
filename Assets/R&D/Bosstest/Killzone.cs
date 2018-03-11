using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    private Collider _killzone;

    private void Reset()
    {
        _killzone = GetComponent<Collider>();
        _killzone.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            var customCollision = other.GetComponent<CustomCollision>();

            if (customCollision != null)
            {
                customCollision.playerMgr.Death();
            }
        }
    }
}
