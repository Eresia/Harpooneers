using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhalePhaseAI : BossAI {

    public GameObject whalePrefab;
    public Transform FX;
    public ParticleSystem spawningFX;

    [Header("Patterns components")]
    public Geyser geyserPrefab;

    public Geyser[] Geysers
    {
        get { return geysers; }
    }
    private Geyser[] geysers;

    public Transform WhaleTransform { get; private set; }
    public Transform WhaleChildTransform { get; private set; }

    public GameObject Whale
    {
        get
        {
            return whale;
        }
    }
    private GameObject whale;
    
    [Header("Patterns")]
    public int NoHittablePatternCount;
    public int numberOfPatternsWithoutHit;

    [Header("Boss attributes")]
    public HandleHarpoonWithEye[] eyes;
    public int hitByEyeNeeded;

    private int passCount = 0;

    private int leftHitCount;
    private int rightHitCount;

    protected override void Awake()
    {
        base.Awake();

        SpawnWhale();
    }

    private void SpawnWhale()
    {
        whale = Instantiate(whalePrefab);

        WhaleTransform = whale.GetComponent<Transform>();
        WhaleChildTransform = WhaleTransform.GetChild(0);

        geysers = new Geyser[GameManager.instance.nbOfPlayers];
        for (int i = 0; i < geysers.Length; i++)
        {
            geysers[i] = Instantiate<Geyser>(geyserPrefab);
        }

        eyes = WhaleChildTransform.GetChild(2).GetComponentsInChildren<HandleHarpoonWithEye>();

        foreach(HandleHarpoonWithEye handleHarpoonHit in eyes)
        {
            handleHarpoonHit.hitCallback = HitEye;
        }
    }

    public int DecideNextPhase()
    {
        passCount++;

        int nextState = 0;
        bool hitPattern = (passCount % (numberOfPatternsWithoutHit + 1)) == 0;

        if (hitPattern)
        {
            nextState = 2;
        }
        else
        {
            nextState = UnityEngine.Random.Range(0, NoHittablePatternCount);
        }

        return nextState;
    }

    public void HitEye(bool left)
    {
        if(left)
        {
            leftHitCount++;
        } else
        {
            rightHitCount++;
        }

        Debug.Log(leftHitCount + " " + rightHitCount);

        // Whale is dead.
        if(leftHitCount >= hitByEyeNeeded && rightHitCount >= hitByEyeNeeded)
        {
            // TODO death feedback
            
            CurrentPattern.StopPattern();

            animator.enabled = false;
            enabled = false;

            OnPhaseFinished();
        }

        // Whale is alive.
        else
        {
            CurrentPattern.StopPattern();
        }
    }
}
