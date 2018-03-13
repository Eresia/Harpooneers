using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Geyser : MonoBehaviour {
    
    public GameObject playerTarget;
    public ParticleSystem bubbleFX;
    public ParticleSystem geyserFX;
    
    public bool IsFinished
    {
        get { return isFinished; }
        set { isFinished = value; }
    }
    private bool isFinished;

    [Space(20)]
    public int numberOfExplosions;
    public float[] moveSpeed;
    public float[] movementDuration;
    public float[] waitingDuration;
    public float[] radius;
    public float[] waveAmplitude;

    public LayerMask damageableLayer;
    
    private Vector3 destinationVector;
    private bool isMoving = true;
    private float elapsedTime = 0;
    private int numberOfExplosionsDone = 0;

    private void OnEnable()
    {
        ResetGeyser();
    }

    void ResetGeyser()
    {
        bubbleFX.Play();

        numberOfExplosionsDone = 0;
        elapsedTime = 0;
        isMoving = true;

        transform.localScale = new Vector3(radius[0], radius[0], radius[0]);
    }


    // Update is called once per frame
    void Update ()
    {
        if(playerTarget == null)
        {
            return;
        }

        // Disable.
        if (numberOfExplosionsDone >= numberOfExplosions)
        {
            gameObject.SetActive(false);
            return;
        }

        // Follow player.
        if (elapsedTime < movementDuration[numberOfExplosionsDone])
        {
            elapsedTime += Time.deltaTime;
            
            destinationVector = new Vector3(playerTarget.transform.position.x, this.transform.position.y, playerTarget.transform.position.z);
            transform.LookAt(destinationVector);
            transform.position += transform.forward * moveSpeed[numberOfExplosionsDone] * Time.deltaTime;

            //Debug.DrawRay(transform.position, destinationVector.normalized * 5f, Color.red, 0.5f);
        }

        // Trigger explosion.
        else
        {
            if(isMoving)
            {
                StartCoroutine(GeyserPause());
            } 
        }
	}

   IEnumerator GeyserPause()
    {
        isMoving = false;
        yield return new WaitForSeconds(waitingDuration[numberOfExplosionsDone]);
        
        bubbleFX.Stop();
        StartCoroutine(GeyserActivation());
    }

    public IEnumerator GeyserActivation()
    {
        GameManager.instance.ground.CreateImpact(transform.position);
        geyserFX.Play();

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius[numberOfExplosionsDone], damageableLayer);
        foreach (Collider c in colliders)
        {
            var customCollision = c.GetComponent<CustomCollision>();

            if (customCollision != null)
            {
                customCollision.playerMgr.Death();
            }
        }

        yield return new WaitWhile(() => (geyserFX.isPlaying));

        numberOfExplosionsDone++;

        if(numberOfExplosionsDone < numberOfExplosions)
        {
            bubbleFX.Play();
            isMoving = true;
            elapsedTime = 0;

            transform.localScale = new Vector3(radius[numberOfExplosionsDone], radius[numberOfExplosionsDone], radius[numberOfExplosionsDone]);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius[0]);
    }
}
