using UnityEngine;

public class SpawnState : BossState<SpawnPattern>
{
    public float depth = 5f;
    
    [Header("Effect duration")]
    [Tooltip("Bubble duration before spawning")]
    public float WaitBeforeSpawn = 2f;

	protected override SpawnPattern Init()
    {
        return new SpawnPattern(this);
	}
}
