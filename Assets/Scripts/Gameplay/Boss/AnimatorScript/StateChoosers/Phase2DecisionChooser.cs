using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2DecisionChooser : BossAnimatorScript
{
    private Phase2AI phase2;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (phase2 == null)
        {
            phase2 = animator.GetComponent<Phase2AI>();

            phase2.bossMgr.DisplayLifeBar(true);
        }

        animator.SetInteger("NextState", phase2.DecideNextPhase());
    }
}
