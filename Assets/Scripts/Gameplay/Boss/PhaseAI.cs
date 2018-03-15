using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Animator))]
public abstract class PhaseAI : MonoBehaviour {

    [Header("Lifepoints and weakpoint config")]
    
    public float maxLifepoints = 100f;

    /// <summary>
    /// Return the actual lifepoints of the whale.
    /// </summary>
    public float Lifepoints
    {
        get { return lifepoints; }
    }
    protected float lifepoints;

    [HideInInspector]
    public BossManager bossMgr;

    protected Animator animator;

    protected BossAnimatorScript[] behaviours;

    protected bool phaseFinished;

    /// <summary>
    /// Return the current pattern played.
    /// </summary>
    public BossPattern CurrentPattern
    {
        get { return currentPattern; }
        set
        {
            currentPattern = value;
        }
    }
    private BossPattern currentPattern;

    public Action OnPhaseFinished;

    public virtual void HitBoss(float damageAmount)
    {
        if (phaseFinished)
        {
            // Don't do anything if boss is defeated.
            return;
        }

        lifepoints -= damageAmount;
        GameManager.instance.bossMgr.UpdateLifepoints();

        if (lifepoints <= 0)
        {
            if (!phaseFinished)
            {
                // TODO death feedback

                phaseFinished = true;

                animator.enabled = false;
                enabled = false;

                StartCoroutine(EndPhaseCoroutine());
            }
        }
    }

	public virtual IEnumerator EndPhaseCoroutine(){
		yield return OnPhaseFinishedCoroutine();
		OnPhaseFinished();
	}

	public abstract IEnumerator OnPhaseFinishedCoroutine();

    protected virtual void Awake()
    {
		animator = GetComponent<Animator>();
		behaviours = animator.GetBehaviours<BossAnimatorScript>();
        
        lifepoints = maxLifepoints;

        foreach (BossAnimatorScript behav in behaviours)
        {
			behav.Init(this, animator);
		}
    }
}
