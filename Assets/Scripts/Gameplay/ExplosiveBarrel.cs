using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IHarpoonable {

    // TODO Use the wave resistance of the bomb stock module.
    
    [Tooltip("Drop distance with the fishing boat")]
    public float behindOffset = 1.25f;
    //public float impactForceNeeded = 2f;

    public BombStockModule bombStockModule;

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

    // Scale fx depending the bomb radius.
    private void SetupFx()
    {
        // TODO Do the same for the _radiusFx
        _explosionFX.transform.localScale = Vector3.one * bombStockModule.bombRadius * 0.5f;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Explosion();
    }

    /// <summary>
    /// Spawn the bomb at a specific position with the specified velocity.
    /// </summary>
    /// <param name="spawnPosition"></param>
    /// <param name="movementDirection"></param>
    public void SpawnTheBomb(Vector3 spawnPosition, Vector3 movementDirection)
    {
        // Spawn Position
        gameObject.transform.position = spawnPosition + new Vector3(0f, 0.25f,0f);

        //Initial Force
        physicsScript.AddForce(movementDirection);

        // Initial Angular Speed
        //_myRigidbody.angularVelocity = new Vector3(0f, Random.Range(-1f, 1f), 0f);

        GetComponent<PhysicMove>().enabled = true;

        _myCollider.enabled = true;

        // Other solution :
        //Invoke("EnableCollider", 0.25f);
    }

    private void EnableCollider()
    {
        _myCollider.enabled = true;
    }

	public void Explosion()
    {
        // TODO : Deal damage with an overlap sphere
        float radiusToUse = bombStockModule.bombRadius;

        // TODO : Shockwave

        //_myRigidbody.angularVelocity = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;

        // Disable physic.
        physicsScript.enabled = false;
        _myRenderer.gameObject.SetActive(false);
        _myCollider.enabled = false;

        _explosionFX.Play();

        StartCoroutine(DeactiveGameObject());
    }

    /// <summary>
    /// Deactive gameobject after that the FX is finished.
    /// </summary>
    /// <returns></returns>
    IEnumerator DeactiveGameObject()
    {
        yield return new WaitWhile(() => (_explosionFX.isPlaying));

        _myRenderer.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    // Debug radius.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bombStockModule.bombRadius);
    }

    // Explode the barrel when harpooned D:
    public void OnHarpoonCollide(Harpoon harpoon)
    {
        harpoon.Cut();

        Explosion();
    }
}