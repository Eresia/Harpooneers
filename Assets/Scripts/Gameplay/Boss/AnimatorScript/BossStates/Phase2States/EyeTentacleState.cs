﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeTentacleState : BossState<EyeTentaclePattern> {

    // Target joueur sur un cercle, emerge, tourne vers le joeuur et tourne en orbite.

    public int attackCount = 2;

    public float bubblingDuration = 5f;
    
    public Vector3 startPos;

    [Header("Emergence")]
    public float emergingDuration = 0.5f;
    public Vector3 attackPos;

    [Header("Attack")]
    public float turnDuration = 1f;
    public float waitBeforeAttack = 3f;

    public float moveSpeed = 10f;

    public float divingDuration = 0.5f;

    [HideInInspector]
    public int tentacleCount = 1;

    protected override EyeTentaclePattern Init()
    {
        return new EyeTentaclePattern(this);
    }
}
