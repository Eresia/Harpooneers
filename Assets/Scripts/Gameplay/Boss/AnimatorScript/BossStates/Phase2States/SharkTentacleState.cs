using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkTentacleState : BossState<SharkTentaclesPattern>
{
    public float bubblingDuration = 3f;

    [Header("Spawn")]
    public Vector3 startPos;

    [Header("Emergence")]
    public float emergingDuration;
    public Vector3 attackPos;
	public bool follow;

    [Header("Attack")]
    public float waitBeforeAttack = 10f;
	public float waitBeforeUp = 2f;
	public float attackHeight = 5f;
	public float attackUpTime = 1f;
	public float waitAfterEat = 4f;
	public float rotationSpeed = 1f;
    public float divingDuration;

    protected override SharkTentaclesPattern Init()
    {
        return new SharkTentaclesPattern(this);
    }
}
