using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Geyser : MonoBehaviour {


    public GameObject playerTarget;
    public ParticleSystem geyserExplosion;


    [Space(20)]
    public float moveSpeed;
    public int numberOfExplosions;
    public float[] movementDuration;
    public float[] waitingDuration;
    public float[] radius;
    public float[] waveAmplitude;

    public LayerMask damageableLayer;

    private Vector3 initialPosition;
    private Vector3 destinationVector;
    private bool isMoving = true;
    private float elapsedTime = 0;
    private int numberOfExplosionsDone = 0;

    


    void Awake()
    {
        initialPosition = transform.position;
    }
    void Start ()
    {
        FollowPlayer();
    }
	
	// Update is called once per frame
	void Update ()
    {


        if (numberOfExplosionsDone >= numberOfExplosions)
        {
            Destroy(gameObject);
        }

        if (elapsedTime < movementDuration[numberOfExplosionsDone])
        {
            elapsedTime += Time.deltaTime;
            transform.LookAt(destinationVector);
            destinationVector = new Vector3(playerTarget.transform.position.x, this.transform.position.y, playerTarget.transform.position.z);        
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
            Debug.DrawRay(transform.position, destinationVector);
        }
        else
        {
            if(isMoving)
                StartCoroutine(GeyserPause());
        }
	}

  public void FollowPlayer()
    {
        
    }

   IEnumerator GeyserPause()
    {
        isMoving = false;
        yield return new WaitForSeconds(waitingDuration[numberOfExplosionsDone]);
        GeyserActivation();
    }

    public void GeyserActivation()
    {
        
        GameManager.instance.ground.CreateImpact(transform.position);
        geyserExplosion.Play();

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius[numberOfExplosionsDone], damageableLayer);
        foreach (Collider c in colliders)
        {
            Debug.Log(colliders.Length);
            var customCollision = GetComponent<CustomCollision>();

            if (customCollision != null)
            {
                customCollision.playerMgr.Death();
            }       
        }

        numberOfExplosionsDone++;

        if (numberOfExplosionsDone >= numberOfExplosions)
        {
            Destroy(gameObject);
        }
        else if(numberOfExplosionsDone < numberOfExplosions)
        {
            isMoving = true;
            elapsedTime = 0;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius[0]);
    }
}
