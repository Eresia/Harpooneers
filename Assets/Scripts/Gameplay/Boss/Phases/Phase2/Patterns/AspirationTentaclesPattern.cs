using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class AspirationTentaclesPattern : BossPattern {

    private AspirationTentacleState state;
    private Phase2AI phase2;

    private TentacleBehaviour[] tentaclesToUse;

	private int tentacleCount;

    public AspirationTentaclesPattern(AspirationTentacleState state)
    {
        this.state = state;
		tentacleCount = 3;
    }

    public override void SetBoss(PhaseAI boss)
    {
        base.SetBoss(boss);

        phase2 = boss as Phase2AI;
    }

    protected override void ExecutePattern()
    {
        boss.StartCoroutine(ActivateTentacles());
    }

    /// <summary>
    /// Spawn 2 tentacles ON a circle with a minimum distance between the 2 tentacles.
    /// </summary>
    private void SpawnTentacle()
    {
        tentaclesToUse = new TentacleBehaviour[tentacleCount];
		Vector3 position = GameManager.instance.bossMgr.east.position;
		position.x -= state.offset;
		tentaclesToUse[1] = phase2.TentacleShark;
		tentaclesToUse[1].transform.position = position;
		tentaclesToUse[1].transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));

		position.z -= state.distanceBeetweenTentacles;
		tentaclesToUse[0] = phase2.TentaclesAspi[0];
		tentaclesToUse[0].transform.position = position;
		tentaclesToUse[0].transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));

		position.z += 2*state.distanceBeetweenTentacles;
		tentaclesToUse[2] = phase2.TentaclesAspi[1];
		tentaclesToUse[2].transform.position = position;
		tentaclesToUse[2].transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
    }

    IEnumerator ActivateTentacles()
    {
        SpawnTentacle();

        for (int i = 0; i < tentacleCount; i++)
        {
            tentaclesToUse[i].Spawning(state.bubblingDuration);
        }

        yield return new WaitForSeconds(state.bubblingDuration);

        for (int i = 0; i < tentacleCount; i++)
        {
            tentaclesToUse[i].Emerge(state.startPos, state.attackPos, state.emergingDuration);
        }

        yield return new WaitForSeconds(state.emergingDuration);

        for (int i = 0; i < tentacleCount; i++)
        {
            tentaclesToUse[i].FeedbackAttackArea();
        }

        yield return new WaitForSeconds(state.waitBeforeAttack);

        for (int i = 0; i < tentacleCount; i++)
        {
            tentaclesToUse[i].TriggerAttackAnim();
        }

        yield return new WaitUntil(() => (tentaclesToUse[0].animator.GetBool("End")));
        
        for (int i = 0; i < tentacleCount; i++)
        {
            tentaclesToUse[i].animator.SetBool("End", false);
            tentaclesToUse[i].attackCollider.enabled = false;

            tentaclesToUse[i].Dive(state.startPos, state.divingDuration);
        }

        yield return new WaitForSeconds(state.divingDuration);

        // Reset pos.
        for (int i = 0; i < tentacleCount; i++)
        {
            tentaclesToUse[i].ResetTentacle();
        }

        yield return null;

        OnPatternFinished();
    }

    protected override void OnStopPattern()
    {
        // DO NOTHING
    }
}
