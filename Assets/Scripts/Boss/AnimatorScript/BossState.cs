using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BossState : StateMachineBehaviour {

	protected BossAI boss;

	protected Animator animator;

	protected Action endPatternAction;

	public virtual void Init(BossAI boss, Animator animator){
		this.boss = boss;
		this.animator = animator;
		this.endPatternAction = new Action(EndPattern);
	}

	public virtual void BeginPattern(BossPattern pattern){
		animator.SetBool("EndPattern", false);
		pattern.ExecutePattern(endPatternAction);
	}

	public virtual void EndPattern(){
		animator.SetBool("EndPattern", true);
	}
	
    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
	// override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		
	// }

	// OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called before OnStateExit is called on any state inside this state machine
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called before OnStateMove is called on any state inside this state machine
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called before OnStateIK is called on any state inside this state machine
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMachineEnter is called when entering a statemachine via its Entry Node
	//override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash){
	//
	//}

	// OnStateMachineExit is called when exiting a statemachine via its Exit Node
	//override public void OnStateMachineExit(Animator animator, int stateMachinePathHash) {
	//
	//}
}
