using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class ChargerTentaclesPattern : BossPattern {

    private ChargerTentacleState state;
    private Phase2AI phase2;

    private Vector3[] spawns;

    private TentacleBehaviour[] tentaclesToUse;

    public ChargerTentaclesPattern(ChargerTentacleState state)
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
        // Spawn 4 tentacles random around center.
        for (int i = 0; i < state.tentacleCount; i++)
        {
            tentaclesToUse[i] = phase2.TentaclesCharger[i];
        }

        // woob wooob woooob
        boss.StartCoroutine(ActivateTentacles());
    }

    /// <summary>
    /// Spawn 2 tentacles ON a circle with a minimum distance between the 2 tentacles.
    /// </summary>
    private void SpawnTentacles()
    {
        /*
        Vector3 center = phase2.bossMgr.center.position;

        // Random a position on a circle (in X and Z).
        Vector3 randPos = Vector3.zero;
        Vector2 randCircle = Random.insideUnitCircle.normalized * state.spawnRadius;
        randPos.x = randCircle.x;
        randPos.z = randCircle.y;

        spawns[0] = center + randPos;

        spawns[1] = RotatePointAroundPivot(spawns[0], Vector3.up, new Vector3(0f, Random.Range(state.minAngle, state.maxAngle), 0f));

        // Store and spawn tentacles.
        tentaclesToUse = new TentacleBehaviour[2];
        for (int i = 0; i < 2; i++)
        {
            tentaclesToUse[i] = phase2.TentaclesHammer[i];

            tentaclesToUse[i].transform.position = spawns[i];

            // Focus the center.
            Vector3 lookCenter = center - spawns[i];
            tentaclesToUse[i].childTransform.localRotation = Quaternion.LookRotation(lookCenter);
        }
        */
    }

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {

        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point

        return point; // return it
    }

    IEnumerator ActivateTentacles()
    {
        yield return new WaitForSeconds(5f);

        OnPatternFinished();
    }

    protected override void OnStopPattern()
    {
        // DO NOTHING
    }
}
