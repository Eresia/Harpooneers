using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimatorScript : StateMachineBehaviour {

	protected PhaseAI boss;

	protected Animator animator;

	public virtual void Init(PhaseAI boss, Animator animator){
		this.boss = boss;
		this.animator = animator;
	}
}
