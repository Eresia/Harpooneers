using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhalePhaseAI : BossAI {

    public GameObject whalePrefab;
    public Transform FX;
    public ParticleSystem spawningFX;

    public Geyser geyserPrefab;

    public Geyser[] Geysers
    {
        get { return geysers; }
    }
    private Geyser[] geysers;

    public Transform WhaleTransform { get; private set; }

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

        geysers = new Geyser[GameManager.instance.nbOfPlayers];
        for (int i = 0; i < geysers.Length; i++)
        {
            geysers[i] = Instantiate<Geyser>(geyserPrefab);
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
        
        // Whale is dead.
        if(leftHitCount == hitByEyeNeeded && rightHitCount == hitByEyeNeeded)
        {

        }

        // Whale is alive.
        else
        {
            CurrentPattern.FinishPattern();
        }
    }
}
