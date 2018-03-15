using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerTentacleState : BossState<ChargerTentaclesPattern> {

    protected override ChargerTentaclesPattern Init()
    {
        return new ChargerTentaclesPattern(this);
    }
}
