using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateChooser : BossAnimatorScript {

	protected abstract int ChooseState();

	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		animator.SetInteger("NextState", ChooseState());
	}
}
