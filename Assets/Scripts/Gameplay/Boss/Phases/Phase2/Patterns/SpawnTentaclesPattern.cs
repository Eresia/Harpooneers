using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpawnTentaclesPattern : BossPattern {

    private SpawnTentacleState state;
    private Phase2AI phase2;

	private TentacleBehaviour[] tentacles;

	private Transform target;

    public SpawnTentaclesPattern(SpawnTentacleState state)
    {
        this.state = state;
    }

    public override void SetBoss(PhaseAI boss)
    {
        base.SetBoss(boss);

        phase2 = boss as Phase2AI;
    }

    protected override void ExecutePattern()
    {
		SpawnTentacle();
		boss.StartCoroutine(ActivateTentacles());
    }

    /// <summary>
    /// Spawn 2 tentacles ON a circle with a minimum distance between the 2 tentacles.
    /// </summary>
    private void SpawnTentacle()
    {
		Vector3 position = GameManager.instance.bossMgr.north.position;
		tentacles = new TentacleBehaviour[5];
		tentacles[0] = phase2.TentaclesAspi[0];
		tentacles[1] = phase2.TentaclesCharger[0];
		tentacles[2] = phase2.TentaclesEye;
		tentacles[3] = phase2.TentaclesHammer[0];
		tentacles[4] = phase2.TentaclesSwipper[0];

		for(int i = -2; i < tentacles.Length - 2; i++){
			tentacles[i+2].transform.position = position - new Vector3(i * state.tentacleSeparations, 0, state.offset);
			tentacles[i+2].transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
		}
	}

    private IEnumerator ActivateTentacles()
    {
		GameManager.instance.shipMgr.LockInputs(1);
		for(int i = 0; i < tentacles.Length; i++){
			tentacles[i].Spawning(state.bubblingDuration);
		}

        yield return new WaitForSeconds(state.bubblingDuration);
		
		for(int i = 0; i < tentacles.Length; i++){
			tentacles[i].Emerge(state.startPos, state.attackPos, state.emergingDuration);
		}

		yield return new WaitForSeconds(state.emergingDuration);

        yield return new WaitForSeconds(state.patternTime);


		for(int i = 0; i < tentacles.Length; i++){
			tentacles[i].animator.SetBool("End", false);
			tentacles[i].Dive(state.startPos, state.divingDuration);
		}

        yield return new WaitForSeconds(state.divingDuration);

		for(int i = 0; i < tentacles.Length; i++){
			tentacles[i].ResetTentacle();
		}
		GameManager.instance.shipMgr.UnLockInputs();

		OnPatternFinished();
    }

    protected override void OnStopPattern()
    {
        
    }
}
