using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class BossAI : MonoBehaviour {

    [HideInInspector]
    public BossManager bossMgr;

    private Animator animator;

	private BossAnimatorScript[] behaviours;

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
