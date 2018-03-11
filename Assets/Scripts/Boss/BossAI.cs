using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class BossAI : MonoBehaviour {

    [HideInInspector]
    public BossManager bossMgr;

    private Animator animator;

	private BossState[] behaviours;

	protected virtual void Awake()
    {
		animator = GetComponent<Animator>();
		behaviours = animator.GetBehaviours<BossState>();

		foreach(BossState behav in behaviours)
        {
			behav.Init(this, animator);
		}
    }
}
