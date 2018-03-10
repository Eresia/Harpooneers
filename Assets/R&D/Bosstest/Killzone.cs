using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    private BoxCollider _killzone;

    private void Awake()
    {
        _killzone = GetComponent<BoxCollider>();
        _killzone.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            other.transform.parent.GetComponent<PlayerManager>().Death();
    }
}
