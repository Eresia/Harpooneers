using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2AI : PhaseAI {

    [Header("Gameobjects of Krakens.")]

    public TentacleReferences tentaclePrefab;
    public int tentaclesNeeded = 4;

    public TentacleReferences[] Tentacles
    {
        get { return tentacles; }
    }
    private TentacleReferences[] tentacles;

    [Header("Patterns and hit")]
    public int hitOnEyesNeeded = 2;
    private int hitOnEyesCount = 0;

    public int bombInMouthNeeded = 2;
    private int bombInMouthCount = 0;

    public int noHittablePatternCount = 2;
    public int[] numberOfPatternsWithoutHit;

    private int step = 0; // Actual step of the phase.

    private int passageCount = 0;

    protected override void Awake()
    {
        base.Awake();

        SpawnTentacles();
    }

    private void SpawnTentacles()
    {
        tentacles = new TentacleReferences[tentaclesNeeded];

        for (int i = 0; i < tentacles.Length; i++)
        {
            tentacles[i] = Instantiate<TentacleReferences>(tentaclePrefab, transform);
            tentacles[i].gameObject.SetActive(false);
        }
    }

    public int DecideNextPhase()
    {
        int nextState = 0;

        passageCount++;
        bool hitPattern = (passageCount % (numberOfPatternsWithoutHit[step] + 1)) == 0;

        if (hitPattern)
        {
            passageCount = 0;

            if(step == 0)
            {
                nextState = 5;
            }

            else
            {
                nextState = 6;
            }
        }

        else
        {
            nextState = Random.Range(0, noHittablePatternCount);
        }

        return nextState;
    }

    public void HitBoss()
    {
        // Handle step eyes.
        if(step == 0)
        {
            hitOnEyesCount++;

            // Step 1 beaten.
            if(hitOnEyesCount >= hitOnEyesNeeded)
            {
                step = 1;
            }
        }

        // Handle step mouth.
        else if(step == 1)
        {
            bombInMouthCount++;
            
            // Phase 2 is beaten.
            if (bombInMouthCount >= bombInMouthNeeded)
            {
                step = 2;

                phaseFinished = true;
                enabled = false;

                animator.enabled = false;

                // TODO death feedback

                OnPhaseFinished();
            }
        }

        GameManager.instance.camMgr.Shake();

        CurrentPattern.StopPattern();
    }
}
