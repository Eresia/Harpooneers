using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerTentacleState : BossState<ChargerTentaclesPattern> {

    public int tentacleCount;
    public float waitBeforeSecondDiagonale = 1f;

    public float bubblingDuration = 1f;

    public Vector3 startPos;
    public float emergingDuration = 0.5f;

    public Vector3 swimPos;
    public float moveSpeed;
    public float offsetWithOther = 5f;

    public float divingDuration = 0.5f;

    protected override ChargerTentaclesPattern Init()
    {
        return new ChargerTentaclesPattern(this);
    }
}
