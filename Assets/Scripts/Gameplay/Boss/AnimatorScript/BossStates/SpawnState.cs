using UnityEngine;

public class SpawnState : BossState<SpawnPattern>
{
	public float offset = 4f;

    public float beginDepth = 10f;

	public float endDepth = 5f;
    
    [Header("Effect duration")]
    [Tooltip("Bubble duration before spawning")]
    public float WaitToSurface = 2f;

	public float WaitOnSurface = 4f;

    public AudioClip spawnSound;

	protected override SpawnPattern Init()
    {
        return new SpawnPattern(this);
	}
}
