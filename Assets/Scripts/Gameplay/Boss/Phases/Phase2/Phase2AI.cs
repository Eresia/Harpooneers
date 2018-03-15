using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2AI : PhaseAI {

    [Header("Gameobjects of Krakens.")]

    public TentacleBehaviour swipperPrefab;
    public TentacleBehaviour hammerPrefab;
    public TentacleBehaviour chargerPrefab;
    public TentacleBehaviour eyeTentaclePrefab;
    public TentacleBehaviour aspiTentaclePrefab;

    public int tentaclesNeeded = 4;

    public TentacleBehaviour[] TentaclesSwipper
    {
        get { return tentaclesSwipper; }
    }
    private TentacleBehaviour[] tentaclesSwipper;

    public TentacleBehaviour[] TentaclesCharger
    {
        get { return tentaclesCharger; }
    }
    private TentacleBehaviour[] tentaclesCharger;

    public TentacleBehaviour[] TentaclesHammer
    {
        get { return tentaclesHammer; }
    }
    private TentacleBehaviour[] tentaclesHammer;

    public TentacleBehaviour[] TentaclesEye
    {
        get { return tentaclesEye; }
    }
    private TentacleBehaviour[] tentaclesEye;

    public TentacleBehaviour[] TentaclesAspi
    {
        get { return tentaclesAspi; }
    }
    private TentacleBehaviour[] tentaclesAspi;

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

        GameManager.instance.shipMgr.UnLockInputs();

        SpawnTentacles();
    }

    private void SpawnTentacles()
    {
        tentaclesSwipper = new TentacleBehaviour[tentaclesNeeded];
        tentaclesHammer = new TentacleBehaviour[tentaclesNeeded];
        tentaclesCharger = new TentacleBehaviour[tentaclesNeeded];
        tentaclesEye = new TentacleBehaviour[tentaclesNeeded];
        tentaclesAspi = new TentacleBehaviour[tentaclesNeeded];

        for (int i = 0; i < tentaclesNeeded; i++)
        {
            tentaclesSwipper[i] = Instantiate<TentacleBehaviour>(swipperPrefab, transform);
            tentaclesSwipper[i].gameObject.SetActive(false);

            tentaclesHammer[i] = Instantiate<TentacleBehaviour>(hammerPrefab, transform);
            tentaclesHammer[i].gameObject.SetActive(false);

            tentaclesCharger[i] = Instantiate<TentacleBehaviour>(chargerPrefab, transform);
            tentaclesCharger[i].gameObject.SetActive(false);

            tentaclesEye[i] = Instantiate<TentacleBehaviour>(eyeTentaclePrefab, transform);
            tentaclesEye[i].gameObject.SetActive(false);

            tentaclesAspi[i] = Instantiate<TentacleBehaviour>(aspiTentaclePrefab, transform);
            tentaclesAspi[i].gameObject.SetActive(false);
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

    public override void HitBoss(float damageAmount)
    {
        base.HitBoss(damageAmount);
    }

	public override IEnumerator OnPhaseFinishedCoroutine(){
		yield return null;
	}
}
