using UnityEngine;

public class RoundState : BossState<RoundPattern>
{    
    [Header("Effect duration")]
    [Tooltip("Bubble duration before spawning")]
    public float depth = 2f;
    
    public float translateSpeed = 20f;

	public float turnSpeed = 1f;

	public float patternTime = 5f;

    private RoundPattern dashPattern;

	protected override RoundPattern Init()
    {
        return new RoundPattern(this);
	}
}
