using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleDecisionChooser : BossAnimatorScript
{
    private WhalePhaseAI whalePhase;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (whalePhase == null)
        {
            whalePhase = animator.GetComponent<WhalePhaseAI>();
        }

        animator.SetInteger("NextState", whalePhase.DecideNextPhase());
    }
}
