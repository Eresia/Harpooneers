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
    public HandleHarpoonWithEye[] eyesScript;
    public int hitByEyeNeeded;

    private int passCount = 0;

    private int leftHitCount;
    private int rightHitCount;

	public AudioClip whale_scream;
	public AudioClip whale_hit;

    protected override void Awake()
    {
        base.Awake();

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
            geysers[i] = Instantiate<Geyser>(geyserPrefab);
        }

        bodyCollider = whaleReferences.bodyCollider;
        tailCollider = whaleReferences.tailCollider;

		GameManager.instance.audioManager.PlaySoundOneTime (whale_scream, 0.2f);

        // Setup eye colliders and scripts.
        eyesScript = whaleReferences.eyeScript;
        eyeColliders = new Collider[eyesScript.Length];

        for (int i = 0; i < eyeColliders.Length; i++)
        {
            // Get and disable by default.
            eyeColliders[i] = eyesScript[i].GetComponent<Collider>();
            eyeColliders[i].enabled = false;

            // Add callback when hit eye.
            eyesScript[i].hitCallback = HitEye;
        }
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

    public void HitEye(bool left)
    {
        if(phaseFinished)
        {
            // Don't do anything if boss is defeated.
            return;
        }

		if(left)
        {
            leftHitCount++;
            whaleReferences.PlayEyeBloodFX(0);
        }

        else
        {
            rightHitCount++;
            whaleReferences.PlayEyeBloodFX(1);
        }

        GameManager.instance.audioManager.PlaySoundOneTime(whale_hit, 0.2f);
        GameManager.instance.audioManager.PlaySoundOneTime(whale_scream, 0.2f);
        GameManager.instance.camMgr.Shake();

        // Whale is dead.
        if(leftHitCount >= hitByEyeNeeded && rightHitCount >= hitByEyeNeeded)
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

        CurrentPattern.StopPattern();
    }

    // Reset the whale's transform.
    public void ResetWhaleTransform()
    {
        Whale.SetActive(false);

        WhaleTransform.rotation = Quaternion.identity;
        WhaleTransform.position = Vector3.zero;
        WhaleTransform.localScale = Vector3.one;
        
        WhaleChildTransform.localRotation = Quaternion.identity;
        WhaleChildTransform.localPosition = Vector3.zero;
        WhaleChildTransform.localScale = Vector3.one;
    }

    // Make whale vulnerable.
    public void EnableEyeCollisions(bool enabled)
    {
        for (int i = 0; i < eyeColliders.Length; i++)
        {
            eyeColliders[i].enabled = enabled;
        }
    }
}
