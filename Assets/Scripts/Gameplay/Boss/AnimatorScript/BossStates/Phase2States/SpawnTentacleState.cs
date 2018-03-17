using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTentacleState : BossState<SpawnTentaclesPattern>
{
    public float bubblingDuration = 3f;

    [Header("Spawn")]
    public Vector3 startPos;
	public float offset = 5f;
	public float tentacleSeparations;

    [Header("Emergence")]
    public float emergingDuration;
	public Vector3 attackPos;

    [Header("Attack")]
    public float patternTime = 5f;
    public float divingDuration;

    protected override SpawnTentaclesPattern Init()
    {
        return new SpawnTentaclesPattern(this);
    }
}
