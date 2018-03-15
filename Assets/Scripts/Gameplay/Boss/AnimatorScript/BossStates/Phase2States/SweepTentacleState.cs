using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepTentacleState : BossState<SweepTentaclesPattern> {

    [HideInInspector]
    public int tentacleCount = 1;

    public float bubblingDuration = 3f;

    public Vector3 startPos = new Vector3(0f, -24f, 0f);

    [Header("Emergence")]
    public float emergingDuration = 0.5f;
    public Vector3 attackPos;

    public float waitBeforeAttack = .5f;

    public float divingDuration = 1f;

    protected override SweepTentaclesPattern Init()
    {
        return new SweepTentaclesPattern(this);
    }
}
