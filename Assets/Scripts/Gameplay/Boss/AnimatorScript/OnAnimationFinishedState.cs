using UnityEngine;

/// <summary>
/// Set the bool "End" to true when the animation of this state finishes.
/// </summary>
public class OnAnimationFinishedState : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("End", true);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        float diff = (animatorStateInfo.length * animatorStateInfo.speed) - animatorStateInfo.normalizedTime;

        if (diff <= 0f)
        {
            animator.SetBool("End", true);
        }
    }
}
