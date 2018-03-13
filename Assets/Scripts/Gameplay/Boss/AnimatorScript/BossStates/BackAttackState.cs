using UnityEngine;

public class BackAttackState : BossState<BackAttackPattern>
{
    public int attackAmount = 3;

    [Header("Emerging")]
    public float emergingDuration = 5f;

    public float startHeight = -50f;

    [Space(10)]

    public float backAttackTime = 0.5f;

    public float waitTimeAfterAttack = 2f;

    [Header("Dive")]
    public float divingDuration = 5f;
    public float diveHeightEnd = -30f;
    public float diveForwardEnd = 20f;

    public Vector3 diveRotationEnd;

    protected override BackAttackPattern Init()
    {
        return new BackAttackPattern(this);
	}
}
