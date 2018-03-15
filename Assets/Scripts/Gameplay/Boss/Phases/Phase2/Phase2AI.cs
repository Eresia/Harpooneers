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
    public TentacleBehaviour tentacleSharkPrefab;

    public int tentaclesNeeded;

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

    public TentacleBehaviour TentaclesEye
    {
        get { return tentaclesEye; }
    }
    private TentacleBehaviour tentaclesEye;

    public TentacleBehaviour[] TentaclesAspi
    {
        get { return tentaclesAspi; }
    }
    private TentacleBehaviour[] tentaclesAspi;

    public TentacleBehaviour TentacleShark
    {
        get { return tentacleShark; }
    }
    private TentacleBehaviour tentacleShark;

    [Header("Patterns and hit")]

    public int noHittablePatternCount = 2;

    [Tooltip("Number of random patterns in each step")]
    public int[] numberOfPatternsWithoutHit;

    public float[] lifepointsToPassStep;

    private int step; // Actual step of the phase.

    private int passageCount;

    protected override void Awake()
    {
        step = 0;
        passageCount = 0;

        base.Awake();

        GameManager.instance.shipMgr.UnLockInputs();

        SpawnTentacles();
    }

    private void SpawnTentacles()
    {
        tentaclesSwipper = new TentacleBehaviour[tentaclesNeeded];
        tentaclesHammer = new TentacleBehaviour[tentaclesNeeded];
        tentaclesCharger = new TentacleBehaviour[tentaclesNeeded];
        tentaclesAspi = new TentacleBehaviour[tentaclesNeeded];

        for (int i = 0; i < tentaclesNeeded; i++)
        {
            tentaclesSwipper[i] = Instantiate<TentacleBehaviour>(swipperPrefab, transform);
            tentaclesSwipper[i].gameObject.SetActive(false);
            tentaclesSwipper[i].harpoonScript.hitCallback = HitBoss;

            tentaclesHammer[i] = Instantiate<TentacleBehaviour>(hammerPrefab, transform);
            tentaclesHammer[i].gameObject.SetActive(false);
            tentaclesHammer[i].harpoonScript.hitCallback = HitBoss;
            
            tentaclesCharger[i] = Instantiate<TentacleBehaviour>(chargerPrefab, transform);
            tentaclesCharger[i].gameObject.SetActive(false);
            tentaclesCharger[i].harpoonScript.hitCallback = HitBoss;

            tentaclesAspi[i] = Instantiate<TentacleBehaviour>(aspiTentaclePrefab, transform);
            tentaclesAspi[i].gameObject.SetActive(false);
            tentaclesAspi[i].harpoonScript.hitCallback = HitBoss;
        }

        tentaclesEye = Instantiate<TentacleBehaviour>(eyeTentaclePrefab, transform);
        tentaclesEye.gameObject.SetActive(false);
        tentaclesEye.harpoonScript.hitCallback = HitBoss;

        tentacleShark = Instantiate<TentacleBehaviour>(tentacleSharkPrefab, transform);
        tentacleShark.gameObject.SetActive(false);
        tentacleShark.harpoonScript.hitCallback = HitBoss;
    }

    public int DecideNextPhase()
    {
        int nextState = 0;

        passageCount++;
        bool hitPattern = (passageCount % (numberOfPatternsWithoutHit[step] + 1)) == 0;

        if (hitPattern)
        {
            passageCount = 0;
            
            nextState = 5 + step;
        }

        else
        {
            nextState = Random.Range(0, noHittablePatternCount);
        }

        return nextState;
    }
    
    public override void HitBoss(float damageAmount)
    {
        base.HitBoss(damageAmount);

        float lifepointsRatio = lifepoints / maxLifepoints;

        if(step < 2)
        {
            if (lifepointsRatio < lifepointsToPassStep[step])
            {
                step++;
                Debug.Log("Next step");
            }
        }
    }

	public override IEnumerator OnPhaseFinishedCoroutine(){
		yield return null;
	}
}
