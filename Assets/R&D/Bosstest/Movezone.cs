using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movezone : MonoBehaviour
{
    private BoxCollider _movezone;
    public Vector3 Offset;

    private void Awake()
    {
        _movezone = GetComponent<BoxCollider>();
        _movezone.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            other.gameObject.transform.position += Offset;
    }
}
