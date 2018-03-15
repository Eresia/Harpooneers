using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerTentacleState : BossState<ChargerTentaclesPattern> {

    public int tentacleCount;

    public Vector3 swimPos;

    protected override ChargerTentaclesPattern Init()
    {
        return new ChargerTentaclesPattern(this);
    }
}
