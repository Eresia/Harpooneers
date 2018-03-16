using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleDecisionChooser : StateChooser
{
    private WhalePhaseAI whalePhase;

	private int actualPhase;

	protected override int ChooseState()
    {
		if (whalePhase == null)
        {
            whalePhase = animator.GetComponent<WhalePhaseAI>();
			actualPhase = 0;

            whalePhase.bossMgr.hasSpawn = true;
            whalePhase.bossMgr.DisplayLifeBar(true);
            return 0;
        }
        
		switch(actualPhase) {
			case 0:
				actualPhase = 1;
				break;
			case 1:
				actualPhase = 0;
				break;
		}

		return actualPhase;
	}
}
