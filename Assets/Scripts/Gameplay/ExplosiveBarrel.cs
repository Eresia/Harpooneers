using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, ICollidable {

    // TODO Use the wave resistance of the bomb stock module.

    public BombStockModule bombStockModule;

    public float impactForceNeeded = 2f;

    private PhysicMove physicsScript;
    private MeshRenderer _myRenderer;
    private ParticleSystem _explosionFX;
    private Collider _myCollider;

    public void Awake()
    {
        transform.parent = null;
        gameObject.SetActive(false);

        physicsScript = GetComponent<PhysicMove>();
        _myRenderer = GetComponentInChildren<MeshRenderer>();
        _explosionFX = GetComponentInChildren<ParticleSystem>();
        _myCollider = GetComponent<Collider>();

        SetupFx();
    }

    private void SetupFx()
    {
        _explosionFX.transform.localScale = Vector3.one * bombStockModule.bombRadius * 0.5f;
    }

    public void OnCollision(Vector3 velocity)
    {
        if(velocity.sqrMagnitude > impactForceNeeded)
        {
            Explosion();
        }
    }

    public void SpawnTheBomb(Vector3 spawnPosition, Vector3 movementDirection)
    {
        // Spawn Position
        gameObject.transform.position = spawnPosition + new Vector3(0f, 0.25f,0f);

        //Initial Force
        physicsScript.AddForce(movementDirection);

        // Initial Angular Speed
        //_myRigidbody.angularVelocity = new Vector3(0f, Random.Range(-1f, 1f), 0f);

        GetComponent<PhysicMove>().enabled = true;
    }

	public void Explosion()
    {
        // TODO : Deal damage with an overlap sphere
        float radiusToUse = bombStockModule.bombRadius;

        // TODO : Shockwave
        // TODO : Play SFX

        //_myRigidbody.angularVelocity = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;

        _myRenderer.gameObject.SetActive(false);
        _myCollider.enabled = false;
        _explosionFX.Play();

        StartCoroutine(DeactiveGameObject());
    }

    IEnumerator DeactiveGameObject()
    {
        yield return new WaitWhile(() => (_explosionFX.isPlaying));

        _myRenderer.gameObject.SetActive(true);
        _myCollider.enabled = true;
        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bombStockModule.bombRadius);
    }
}