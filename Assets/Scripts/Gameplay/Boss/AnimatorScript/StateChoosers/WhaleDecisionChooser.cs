using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleDecisionChooser : StateChooser
{
    private WhalePhaseAI whalePhase;

	protected override int ChooseState(){
		if (whalePhase == null)
        {
            whalePhase = animator.GetComponent<WhalePhaseAI>();
        }

		// return whalePhase.DecideNextPhase();
		return 0;
	}
}
