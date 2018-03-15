using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspirationTentacleState : BossState<AspirationTentaclesPattern>
{
    public int attackCount = 2;

    public float bubblingDuration = 3f;

    [Header("Spawn")]
    public float spawnRadius = 10f;
    public float minAngle = 45f;
    public float maxAngle = 315f;
	public float offset = 1f;
	public float distanceBeetweenTentacles = 2f;

    public Vector3 startPos;

    [Header("Emergence")]
    public float emergingDuration;
    public Vector3 attackPos;

    [Header("Attack")]
    public float turnDuration = 1f;
    public float waitBeforeAttack = 0.5f;

    public float divingDuration;

    protected override AspirationTentaclesPattern Init()
    {
        return new AspirationTentaclesPattern(this);
    }
}
