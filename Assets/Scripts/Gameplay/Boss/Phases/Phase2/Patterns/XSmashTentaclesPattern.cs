using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XSmashTentaclesPattern : BossPattern {

    private XSmashTentacleState state;
    private Phase2AI phase2;

    private Vector3[] spawns;

    private TentacleBehaviour[] tentaclesToUse;

    public XSmashTentaclesPattern(XSmashTentacleState state)
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
        // Spawn 2 tentacles random around center.
        
        // woob wooob woooob
        boss.StartCoroutine(ActivateTentacles());
    }

    /// <summary>
    /// Spawn 2 tentacles ON a circle with a minimum distance between the 2 tentacles.
    /// </summary>
    private void SpawnCross()
    {
        spawns[0] = boss.bossMgr.east.position;
        spawns[1] = boss.bossMgr.north.position;
        spawns[2] = boss.bossMgr.west.position;
        spawns[3] = boss.bossMgr.south.position;

        Vector3 center = phase2.bossMgr.center.position;

        // Store and spawn tentacles.
        tentaclesToUse = new TentacleBehaviour[state.tentacleCount];
        for (int i = 0; i < state.tentacleCount; i++)
        {
            tentaclesToUse[i] = phase2.TentaclesHammer[i];

            tentaclesToUse[i].transform.position = spawns[i];

            // Focus the center.
            Vector3 lookCenter = center - spawns[i];
            tentaclesToUse[i].childTransform.localRotation = Quaternion.LookRotation(lookCenter);
        }
    }

    /// <summary>
    /// Spawn 2 tentacles ON a circle with a minimum distance between the 2 tentacles.
    /// </summary>
    private void SpawnCorners()
    {
        spawns[0] = new Vector3(boss.bossMgr.east.position.x, 0f, boss.bossMgr.north.position.z);
        spawns[1] = new Vector3(boss.bossMgr.west.position.x, 0f, boss.bossMgr.south.position.z);
        spawns[2] = new Vector3(boss.bossMgr.east.position.x, 0f, boss.bossMgr.south.position.z);
        spawns[3] = new Vector3(boss.bossMgr.west.position.x, 0f, boss.bossMgr.north.position.z);

        Vector3 center = phase2.bossMgr.center.position;

        // Store and spawn tentacles.
        tentaclesToUse = new TentacleBehaviour[state.tentacleCount];
        for (int i = 0; i < state.tentacleCount; i++)
        {
            tentaclesToUse[i] = phase2.TentaclesHammer[i];

            tentaclesToUse[i].transform.position = spawns[i];

            // Focus the center.
            Vector3 lookCenter = center - spawns[i];
            tentaclesToUse[i].childTransform.localRotation = Quaternion.LookRotation(lookCenter);
        }
    }

    IEnumerator ActivateTentacles()
    {
        for (int attack = 0; attack < 2; attack++)
        {
            if (attack == 0)
            {
                SpawnCross();
            }

            else
            {
                SpawnCorners();
            }

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

                tentaclesToUse[i].Dive(state.startPos, state.divingDuration);
            }

            yield return new WaitForSeconds(state.divingDuration);

            // Reset pos.
            for (int i = 0; i < state.tentacleCount; i++)
            {
                tentaclesToUse[i].ResetTentacle();
            }
        }

        OnPatternFinished();
    }

    protected override void OnStopPattern()
    {
        // DO NOTHING
    }
}
