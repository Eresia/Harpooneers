using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimatorScript : StateMachineBehaviour {

	protected BossAI boss;

	protected Animator animator;

	public virtual void Init(BossAI boss, Animator animator){
		this.boss = boss;
		this.animator = animator;
	}
}
