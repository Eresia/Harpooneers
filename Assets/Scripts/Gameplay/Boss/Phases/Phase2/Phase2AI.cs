using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2AI : PhaseAI {

    public int hitOnEyesNeeded = 2;
    private int hitOnEyesCount = 0;

    public int bombInMouthNeeded = 2;
    private int bombInMouthCount = 0;

    public int noHittablePatternCount;
    public int[] numberOfPatternsWithoutHit;

    private int step = 0; // Actual step of the phase.

    private int passageCount = 0;

    public int DecideNextPhase()
    {
        int nextState = 0;

        passageCount++;
        bool hitPattern = (passageCount % (numberOfPatternsWithoutHit[step] + 1)) == 0;

        if (hitPattern)
        {
            passageCount = 0;

            if(step == 0)
            {
                nextState = 5;
            }

            else
            {
                nextState = 6;
            }
        }

        else
        {
            nextState = UnityEngine.Random.Range(0, noHittablePatternCount);
        }

        return nextState;
    }

    public void HitBoss()
    {
        if(step == 0)
        {
            hitOnEyesCount++;

            if(hitOnEyesCount >= hitOnEyesNeeded)
            {
                step = 1;
            }
        }

        else if(step == 1)
        {
            bombInMouthCount++;
            
            // Phase 2 is beaten.
            if (bombInMouthCount >= bombInMouthNeeded)
            {
                step = 2;

                phaseFinished = true;
                enabled = false;

                animator.enabled = false;

                // TODO death feedback

                OnPhaseFinished();
            }
        }

        GameManager.instance.camMgr.Shake();

        CurrentPattern.StopPattern();
    }
}
