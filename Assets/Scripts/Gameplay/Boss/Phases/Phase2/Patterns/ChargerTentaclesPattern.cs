using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class ChargerTentaclesPattern : BossPattern {

    private ChargerTentacleState state;
    private Phase2AI phase2;
    
    private TentacleBehaviour[] tentaclesToUse;
    private Vector3[] spawns;
    private Vector3[] destinations;

    public ChargerTentaclesPattern(ChargerTentacleState state)
    {
        this.state = state;

        spawns = new Vector3[state.tentacleCount];
        destinations = new Vector3[state.tentacleCount];
        tentaclesToUse = new TentacleBehaviour[state.tentacleCount];
    }

    public override void SetBoss(PhaseAI boss)
    {
        base.SetBoss(boss);

        phase2 = boss as Phase2AI;
    }

    protected override void ExecutePattern()
    {
        SpawnTentacles();

        boss.StartCoroutine(Diagonale1());

        boss.StartCoroutine(Diagonale2());
    }

    /// <summary>
    /// Spawn 2 tentacles ON a circle with a minimum distance between the 2 tentacles.
    /// </summary>
    private void SpawnTentacles()
    {
        Vector3 center = phase2.bossMgr.center.position;

        spawns[0] = new Vector3(boss.bossMgr.east.position.x, 0f, boss.bossMgr.north.position.z);
        spawns[1] = new Vector3(boss.bossMgr.west.position.x, 0f, boss.bossMgr.south.position.z);

        spawns[2] = new Vector3(boss.bossMgr.east.position.x, 0f, boss.bossMgr.south.position.z);
        spawns[3] = new Vector3(boss.bossMgr.west.position.x, 0f, boss.bossMgr.north.position.z);

        // Spawn 4 tentacles around center.
        for (int i = 0; i < state.tentacleCount; i++)
        {
            tentaclesToUse[i] = phase2.TentaclesCharger[i];
            tentaclesToUse[i].transform.position = spawns[i];

            // Focus the center.
            Vector3 lookCenter = center - tentaclesToUse[i].transform.position;
            tentaclesToUse[i].childTransform.localRotation = Quaternion.LookRotation(lookCenter);

            // Move to don't collide with the other side.
            int left = (i % 2 == 0) ? -1 : 1;
            tentaclesToUse[i].transform.Translate(tentaclesToUse[i].transform.right * state.offsetWithOther * left);
        }
    }

    IEnumerator Diagonale1()
    {
        int tentacleCountHere = state.tentacleCount / 2;

        for (int i = 0; i < tentacleCountHere; i++)
        {
            tentaclesToUse[i].Spawning(state.bubblingDuration);
        }

        yield return new WaitForSeconds(state.bubblingDuration);

        for (int i = 0; i < tentacleCountHere; i++)
        {
            tentaclesToUse[i].Emerge(state.startPos, state.swimPos, state.emergingDuration);
        }

        yield return new WaitForSeconds(state.emergingDuration);
        
        float duration = 0f;

        for (int i = 0; i < tentacleCountHere; i++)
        {
            destinations[i] = (phase2.bossMgr.center.position - spawns[i]) * 2;

            duration = destinations[i].magnitude / state.moveSpeed;

            tentaclesToUse[i].childTransform.DOLocalMove(destinations[i], duration).SetEase(Ease.Linear);
        }

        yield return new WaitForSeconds(duration);

        for (int i = 0; i < tentacleCountHere; i++)
        {
            tentaclesToUse[i].Dive(destinations[i] + state.startPos, state.divingDuration);
        }

        yield return new WaitForSeconds(state.divingDuration);

        // Reset pos.
        for (int i = 0; i < tentacleCountHere; i++)
        {
            tentaclesToUse[i].ResetTentacle();
        }
    }

    IEnumerator Diagonale2()
    {
        yield return new WaitForSeconds(state.waitBeforeSecondDiagonale);

        int tentacleCountHere = state.tentacleCount;

        for (int i = 2; i < tentacleCountHere; i++)
        {
            tentaclesToUse[i].Spawning(state.bubblingDuration);
        }

        yield return new WaitForSeconds(state.bubblingDuration);

        for (int i = 2; i < tentacleCountHere; i++)
        {
            tentaclesToUse[i].Emerge(state.startPos, state.swimPos, state.emergingDuration);
        }

        yield return new WaitForSeconds(state.emergingDuration);

        float duration = 0f;

        for (int i = 2; i < tentacleCountHere; i++)
        {
            destinations[i] = (phase2.bossMgr.center.position - spawns[i]) * 2;

            duration = destinations[i].magnitude / state.moveSpeed;

            tentaclesToUse[i].childTransform.DOLocalMove(destinations[i], duration).SetEase(Ease.Linear);
        }

        yield return new WaitForSeconds(duration);

        for (int i = 2; i < tentacleCountHere; i++)
        {
            tentaclesToUse[i].Dive(destinations[i] + state.startPos, state.divingDuration);
        }

        yield return new WaitForSeconds(state.divingDuration);

        // Reset pos.
        for (int i = 2; i < tentacleCountHere; i++)
        {
            tentaclesToUse[i].ResetTentacle();
        }

        OnPatternFinished();
    }

    protected override void OnStopPattern()
    {
        // DO NOTHING
    }
}
