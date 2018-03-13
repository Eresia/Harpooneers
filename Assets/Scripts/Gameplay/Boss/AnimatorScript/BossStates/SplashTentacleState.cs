﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashTentacleState : BossState<SplashTentaclesPattern>
{
    public int attackCount = 2;

    public int tentacleCount = 5;

    public float bubblingDuration = 3f;

    [Header("Spawn")]
    public float spawnRadius = 10f;
    public Vector3 startPos;

    [Header("Emergence")]
    public float emergingDuration;
    public Vector3 attackPos;

    [Header("Targetting")]
    public float turnDuration = 1f;

    public float divingDuration;

    protected override SplashTentaclesPattern Init()
    {
        return new SplashTentaclesPattern(this);
    }
}