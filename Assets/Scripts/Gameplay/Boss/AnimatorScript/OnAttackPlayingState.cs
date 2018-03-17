using UnityEngine;

public class OnAttackPlayingState : StateMachineBehaviour
{
    public float attackTimeMin = 0.4f;
    public float attackTimeMax = 1f;

    public override void OnStateMove(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        float animTime = animatorStateInfo.normalizedTime;

        bool isAttacking = false;
        if (animTime >= attackTimeMin && animTime <= attackTimeMax)
        {
            isAttacking = true;
        }

        animator.SetBool("IsAttacking", isAttacking);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("End", true);
    }
}
