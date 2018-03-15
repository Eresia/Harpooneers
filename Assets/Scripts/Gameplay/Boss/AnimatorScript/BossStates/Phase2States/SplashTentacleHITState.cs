using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashTentacleHITState : BossState<SplashTentaclesHITPattern> {

    // Target joueur sur un cercle, emerge, tourne vers le joeuur et tourne en orbite.

    [HideInInspector]
    public int tentacleCount = 1;

    protected override SplashTentaclesHITPattern Init()
    {
        return new SplashTentaclesHITPattern(this);
    }
}
