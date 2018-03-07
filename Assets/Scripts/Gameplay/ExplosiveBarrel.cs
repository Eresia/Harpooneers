﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour {


    public float forceResistance;
    private Rigidbody _myRigidbody;
    private MeshRenderer _myRenderer;
    private ParticleSystem _explosionFX;
    private SphereCollider _myCollider;

    public void Start()
    {
        transform.parent = null;
        gameObject.SetActive(false);

        _myRigidbody = GetComponent<Rigidbody>();
        _myRenderer = GetComponentInChildren<MeshRenderer>();
        _explosionFX = GetComponentInChildren<ParticleSystem>();
        _myCollider = GetComponent<SphereCollider>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.impulse.magnitude > forceResistance)
        {
            Explosion();
        }
    }

    public void SpawnTheBomb(Vector3 spawnPosition, Vector3 movementDirection)
    {
        // Spawn Position
        gameObject.transform.position = spawnPosition + new Vector3(0f, 0.25f,0f);

        //Initial Force
        _myRigidbody.AddForce(movementDirection, ForceMode.Impulse);

        // Initial Angular Speed
        _myRigidbody.angularVelocity = new Vector3(0f, Random.Range(-1f, 1f), 0f);
    }

   

	public void Explosion()
    {
        // TODO : Deal damage
        // TODO : Shockwave
        // TODO : Play SFX

        _myRigidbody.angularVelocity = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;

        _myRenderer.gameObject.SetActive(false);
        _myCollider.enabled = false;
        _explosionFX.Play();

        StartCoroutine(DeactiveGameObject());
    }

    IEnumerator DeactiveGameObject()
    {
        yield return new WaitForSeconds(1.5f);
        _myRenderer.gameObject.SetActive(true);
        _myCollider.enabled = true;
        gameObject.SetActive(false);
    }
}