using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BossState<T> : BossAnimatorScript where T : BossPattern {

	protected Action endPatternAction;

	protected T pattern;

	public override void Init(BossAI boss, Animator animator)
    {
		base.Init(boss, animator);

		this.endPatternAction = new Action(EndPattern);
		this.pattern = Init();
        this.pattern.SetBoss(boss);
	}

	protected abstract T Init();

	public virtual void BeginPattern()
    {
		animator.SetBool("EndPattern", false);
		pattern.ExecutePattern(endPatternAction);
	}

	public virtual void EndPattern()
    {
		animator.SetBool("EndPattern", true);
	}
	
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		BeginPattern();
	}
}
