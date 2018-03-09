using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour, IResetable {

    // TODO Use the wave resistance of the bomb stock module.
    
    [Tooltip("Drop distance with the fishing boat")]
    public float behindOffset = 1.25f;
    //public float impactForceNeeded = 2f;

    [Header("Bomb config")]
    public BombStockModule bombStockModule;
    public LayerMask damageableLayer;
    public float delayWhenExplodeInChain = 0.5f;

    [Header("FX")]
    public GameObject radiusFX;
    public ParticleSystem explosionFX;
    public ParticleSystem fuseFX;

    [Header("Other components")]
    public ResetWhenLeaveScreen resetWhenLeaveScreen;
    public PhysicMove physicsScript;

    private MeshRenderer _myRenderer;
    private Collider _myCollider;

    private bool hasAlreadyExplode;

    public void Awake()
    {
        transform.parent = null;
        gameObject.SetActive(false);

        physicsScript = GetComponent<PhysicMove>();
        _myRenderer = GetComponentInChildren<MeshRenderer>();
        _myCollider = GetComponent<Collider>();

        resetWhenLeaveScreen.resetable = this;
    }

    // Scale fx depending the bomb radius.
    public void SetupBombFX()
    {
        Vector3 resize = Vector3.one * bombStockModule.bombRadius;
        
        radiusFX.transform.parent.localScale *= bombStockModule.bombRadius;
        explosionFX.transform.localScale = resize;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        TriggerExplosion(0f);
    }

    /// <summary>
    /// Spawn the bomb at a specific position with the specified velocity.
    /// </summary>
    /// <param name="spawnPosition"></param>
    /// <param name="movementDirection"></param>
    public void SpawnTheBomb(Vector3 spawnPosition, Vector3 movementDirection)
    {
        hasAlreadyExplode = false;

        radiusFX.SetActive(true);

        // Spawn Position
        gameObject.transform.position = spawnPosition + new Vector3(0f, 0.25f,0f);

        //Initial Force
        physicsScript.AddForce(movementDirection);

        // Initial Angular Speed
        //_myRigidbody.angularVelocity = new Vector3(0f, Random.Range(-1f, 1f), 0f);

        GetComponent<PhysicMove>().enabled = true;

        _myCollider.enabled = true;
    }

    public void TriggerExplosion(float delayToExplode)
    {
        if(hasAlreadyExplode)
        {
            return;
        }

        hasAlreadyExplode = true;

        StartCoroutine(Explosion(delayToExplode));
    }

    private IEnumerator Explosion(float delay)
    {
        if (delay > 0)
            fuseFX.Play();

        yield return new WaitForSeconds(delay);

        // Deal damage with an overlap sphere
        Collider[] colliders = Physics.OverlapSphere(transform.position, bombStockModule.bombRadius, damageableLayer);
        foreach (Collider c in colliders)
        {
            if (c == _myCollider)
            {
                continue;
            }

            c.SendMessage("OnExplode", SendMessageOptions.DontRequireReceiver);
        }

        // TODO : Shockwave on the sea.
        GameManager.instance.ground.CreateImpact(transform.position);

        //_myRigidbody.angularVelocity = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;

        // Disable physic.
        physicsScript.enabled = false;
        _myRenderer.gameObject.SetActive(false);
        _myCollider.enabled = false;

        // Clear and stop to debug the radius.
        radiusFX.SetActive(false);

        explosionFX.Play();
        fuseFX.Stop();

        StartCoroutine(DeactiveGameObject());
    }

    public void OnExplode()
    {
        Debug.Log(gameObject + " EXPLODE");

        if(hasAlreadyExplode)
        {
            return;
        }

        TriggerExplosion(delayWhenExplodeInChain);
    }

    /// <summary>
    /// Deactive gameobject after that the FX is finished.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeactiveGameObject()
    {
        yield return new WaitWhile(() => (explosionFX.isPlaying));

        _myRenderer.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    // Debug radius.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bombStockModule.bombRadius);
    }

    public void ResetGameObject()
    {
        Debug.Log("RESET BARREL");

        StartCoroutine(DeactiveGameObject());
        resetWhenLeaveScreen.isReseting = false;
    }
}