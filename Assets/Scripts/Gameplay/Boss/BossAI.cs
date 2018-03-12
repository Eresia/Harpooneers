using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class BossAI : MonoBehaviour {

    [HideInInspector]
    public BossManager bossMgr;

    protected Animator animator;

    protected BossAnimatorScript[] behaviours;

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

    protected virtual void Awake()
    {
		animator = GetComponent<Animator>();
		behaviours = animator.GetBehaviours<BossAnimatorScript>();

		foreach(BossAnimatorScript behav in behaviours)
        {
			behav.Init(this, animator);
		}
    }
}
