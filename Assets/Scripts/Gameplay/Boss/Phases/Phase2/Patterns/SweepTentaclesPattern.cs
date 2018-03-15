using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class SweepTentaclesPattern : BossPattern {

    private SweepTentacleState state;
    private Phase2AI phase2;

    private Vector3[] spawns;

    private TentacleBehaviour[] tentaclesToUse;

    public SweepTentaclesPattern(SweepTentacleState state)
    {
        this.state = state;

        spawns = new Vector3[state.tentacleCount];
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
        Vector3 center = GameManager.instance.shipMgr.ChoosePlayerToAttack().position;

        // Random a position on a circle (in X and Z).
        Vector3 randPos = Vector3.zero;
        Vector2 randCircle = Random.insideUnitCircle.normalized * 5f;
        randPos.x = randCircle.x;
        randPos.z = randCircle.y;

        spawns[0] = center + randPos;

        // Store and spawn tentacles.
        tentaclesToUse = new TentacleBehaviour[state.tentacleCount];
        for (int i = 0; i < state.tentacleCount; i++)
        {
            tentaclesToUse[i] = phase2.TentaclesSwipper[i];

            tentaclesToUse[i].transform.position = spawns[i];

            // Focus the center.
            Vector3 lookCenter = center - spawns[i];
            tentaclesToUse[i].childTransform.localRotation = Quaternion.LookRotation(lookCenter);
        }
    }

    IEnumerator ActivateTentacles()
    {
        SpawnTentacle();

        for (int i = 0; i < state.tentacleCount; i++)
        {
            tentaclesToUse[i].Spawning(state.bubblingDuration);
        }

        yield return new WaitForSeconds(state.bubblingDuration);

        for (int i = 0; i < state.tentacleCount; i++)
        {
            tentaclesToUse[i].Emerge(state.startPos, state.attackPos, state.emergingDuration);
        }

        yield return new WaitForSeconds(state.emergingDuration);

        for (int i = 0; i < state.tentacleCount; i++)
        {
            tentaclesToUse[i].FeedbackAttackArea();
        }

        yield return new WaitForSeconds(state.waitBeforeAttack);

        for (int i = 0; i < state.tentacleCount; i++)
        {
            tentaclesToUse[i].TriggerAttackAnim();
        }

        yield return new WaitUntil(() => (tentaclesToUse[0].animator.GetBool("End")));
        
        for (int i = 0; i < state.tentacleCount; i++)
        {
            tentaclesToUse[i].animator.SetBool("End", false);
            tentaclesToUse[i].attackCollider.enabled = false;

            tentaclesToUse[i].Dive(state.startPos, state.divingDuration);
        }

        yield return new WaitForSeconds(state.divingDuration);

        // Reset pos.
        for (int i = 0; i < state.tentacleCount; i++)
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
