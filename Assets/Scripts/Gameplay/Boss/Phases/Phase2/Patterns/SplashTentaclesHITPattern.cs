using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class SplashTentaclesHITPattern : BossPattern {

    private SplashTentacleHITState state;
    private Phase2AI phase2;

    private Vector3[] spawns;

    private TentacleBehaviour[] tentaclesToUse;

    public SplashTentaclesHITPattern(SplashTentacleHITState state)
    {
        this.state = state;

        spawns = new Vector3[state.tentacleCount];
        tentaclesToUse = new TentacleBehaviour[state.tentacleCount];
    }

    public override void SetBoss(PhaseAI boss)
    {
        base.SetBoss(boss);

        phase2 = boss as Phase2AI;
    }

    protected override void ExecutePattern()
    {
        // Spawn 2 tentacles random around center.

        SpawnTentacles();
        
        // woob wooob woooob
        boss.StartCoroutine(ActivateTentacles());
    }

    /// <summary>
    /// Spawn 2 tentacles ON a circle with a minimum distance between the 2 tentacles.
    /// </summary>
    private void SpawnTentacles()
    {
        spawns[0] = boss.bossMgr.center.position;

        tentaclesToUse[0] = phase2.TentaclesEye;
        tentaclesToUse[0].transform.position = spawns[0];

        Vector3 south = boss.bossMgr.south.position;
        Vector3 dir = south - spawns[0];
    }

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {

        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point

        return point; // return it
    }

    IEnumerator ActivateTentacles()
    {
        for (int attack = 0; attack < state.attackCount; attack++)
        {
            SpawnTentacles();

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

            // Each tentacles focus a player.
            for (int i = 0; i < state.tentacleCount; i++)
            {
                tentaclesToUse[i].FocusPlayer(state.turnDuration);
            }

            yield return new WaitForSeconds(state.turnDuration);

            yield return new WaitForSeconds(state.waitBeforeAttack);

            for (int i = 0; i < state.tentacleCount; i++)
            {
                tentaclesToUse[i].TriggerAttackAnim();
            }

            yield return new WaitUntil(() => (tentaclesToUse[0].animator.GetBool("End")));
            
            for (int i = 0; i < state.tentacleCount; i++)
            {
                tentaclesToUse[i].animator.SetBool("End", false);

                tentaclesToUse[i].Dive(state.startPos, state.divingDuration);
            }

            yield return new WaitForSeconds(state.divingDuration);

            // Reset pos.
            for (int i = 0; i < state.tentacleCount; i++)
            {
                tentaclesToUse[i].ResetTentacle();
            }
        }

        yield return null;

        OnPatternFinished();
    }

    protected override void OnStopPattern()
    {
        // DO NOTHING
    }
}
