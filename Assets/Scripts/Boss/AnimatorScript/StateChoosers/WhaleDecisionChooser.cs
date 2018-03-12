using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleDecisionChooser : BossAnimatorScript
{
    public int NoHittablePatternCount;
    public int numberOfpatternsWithoutHit;

    private WhalePhaseAI whalePhase;

    private int passCount = 0;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (whalePhase == null)
        {
            whalePhase = animator.GetComponent<WhalePhaseAI>();
        }

        passCount++;

        int nextState = 0;
        bool hitPattern = (passCount % (numberOfpatternsWithoutHit + 1)) == 0;

        if (hitPattern)
        {
            nextState = 2;
        }
        else
        {
            nextState = Random.Range(0, NoHittablePatternCount);
        }

        animator.SetInteger("NextState", nextState);
    }
}
