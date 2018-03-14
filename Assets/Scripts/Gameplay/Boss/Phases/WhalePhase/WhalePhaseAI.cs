using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhalePhaseAI : PhaseAI {

    public GameObject whalePrefab;
    public Transform FX;
    public ParticleSystem spawningFX;

    [HideInInspector]
    public WhaleReferences whaleReferences;

    [Header("Patterns components")]
    public Geyser geyserPrefab;

    public Geyser[] Geysers
    {
        get { return geysers; }
    }
    private Geyser[] geysers;

    public Transform WhaleTransform { get; private set; }
    public Transform WhaleChildTransform { get; private set; }
    public Animator WhaleAnimator { get; private set; }

    public GameObject Whale
    {
        get
        {
            return whale;
        }
    }
    private GameObject whale;
    
    [Header("Patterns")]
    public int noHittablePatternCount;
    public int numberOfPatternsWithoutHit;

    [Header("Boss attributes")]

    public Collider[] eyeColliders;
    public Collider bodyCollider;
    public Collider tailCollider;

    public HandleHarpoonWithWhale[] whaleScript;
    
    private int passCount = 0;

    public float weakPointDamageMinAmount = 10f;

    public float maxLifepoints = 100f;
    
    private float lifepoints;

	public AudioClip whale_scream;

	public float resetDepth;

    protected override void Awake()
    {
        base.Awake();

        lifepoints = maxLifepoints;

        SpawnWhale();
    }

    // Spawn and setup whale correctly.
    private void SpawnWhale()
    {
        whale = Instantiate(whalePrefab);
        whale.SetActive(false);

        whaleReferences = whale.GetComponent<WhaleReferences>();
        WhaleTransform = whale.GetComponent<Transform>();
        WhaleChildTransform = whaleReferences.whaleChildTransform;
        WhaleAnimator = whaleReferences.whaleAnimator;

        geysers = new Geyser[GameManager.instance.nbOfPlayers];
        for (int i = 0; i < geysers.Length; i++)
        {
            geysers[i] = Instantiate(geyserPrefab);
        }

        bodyCollider = whaleReferences.bodyCollider;
        tailCollider = whaleReferences.tailCollider;

		whaleReferences.whaleBody.OnWhaleExplode = StopPattern;

		GameManager.instance.audioManager.PlaySoundOneTime (whale_scream, 0.2f);

        // Setup eye colliders and scripts.
        whaleScript = whaleReferences.hittableScripts;
        eyeColliders = new Collider[whaleScript.Length];

        for (int i = 0; i < whaleReferences.hittableScripts.Length; i++)
        {
            whaleReferences.hittableScripts[i].hitCallback = HitWhale;
        }
    }

	public void StopPattern(){
		CurrentPattern.StopPattern();
	}

    public int DecideNextPhase()
    {
        passCount++;

        int nextState = 0;
        bool hitPattern = (passCount % (numberOfPatternsWithoutHit + 1)) == 0;

        if (hitPattern)
        {
            passCount = 0;
            nextState = 2;
        }
        else
        {
            nextState = UnityEngine.Random.Range(0, noHittablePatternCount);
        }

        return nextState;
    }

    public void HitWhale(float damageAmount)
    {
        if(phaseFinished)
        {
            // Don't do anything if boss is defeated.
            return;
        }

        if(damageAmount >= weakPointDamageMinAmount)
        {
            GameManager.instance.audioManager.PlaySoundOneTime(whale_scream, 0.2f);
            GameManager.instance.camMgr.Shake();
        }

        lifepoints -= damageAmount;
        Debug.Log(lifepoints);
        //lifepoints = Mathf.Clamp(lifepoints, 0f, maxLifepoints);

        // No any PVs
        if(lifepoints <= 0)
        {
            if(!phaseFinished)
            {
                // TODO death feedback

                phaseFinished = true;

                animator.enabled = false;
                enabled = false;
                
                OnPhaseFinished();
            }

            return;
        }

        StopPattern();
    }

    // Reset the whale's transform.
    public void ResetWhaleTransform()
    {
        ResetWhaleTransform(resetDepth);
    }

	public void ResetWhaleTransform(float depth)
    {
        Whale.SetActive(false);

        WhaleTransform.rotation = Quaternion.identity;
        WhaleTransform.position = new Vector3(0, -depth, 0);
        WhaleTransform.localScale = Vector3.one;
        
        WhaleChildTransform.localRotation = Quaternion.identity;
        WhaleChildTransform.localPosition = Vector3.zero;
        WhaleChildTransform.localScale = Vector3.one;
    }
}
