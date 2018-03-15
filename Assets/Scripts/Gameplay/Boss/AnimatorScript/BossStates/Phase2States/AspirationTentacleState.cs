using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspirationTentacleState : BossState<AspirationTentaclesPattern>
{
    public float bubblingDuration = 3f;

    [Header("Spawn")]
	public float offset = 1f;
	public float distanceBeetweenTentacles = 2f;
	public float waveDuration = 2f;
	public float waveAmplitude = 3f;

    public Vector3 startPos;

    [Header("Emergence")]
    public float emergingDuration;
    public Vector3 attackPos;

    [Header("Attack")]
    public float waitBeforeAttack = 0.5f;

	[Space]
	public float maxTime = 20f;

    public float divingDuration;

    protected override AspirationTentaclesPattern Init()
    {
        return new AspirationTentaclesPattern(this);
    }
}
