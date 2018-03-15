﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XSmashTentacleState : BossState<XSmashTentaclesPattern> {

    [HideInInspector]
    public int tentacleCount = 4;

    public float bubblingDuration = 3f;

    public Vector3 startPos;

    [Header("Emergence")]
    public float emergingDuration;
    public Vector3 attackPos;

    public float waitBeforeAttack = .5f;
    
    public float divingDuration;

    protected override XSmashTentaclesPattern Init()
    {
        return new XSmashTentaclesPattern(this);
    }
}