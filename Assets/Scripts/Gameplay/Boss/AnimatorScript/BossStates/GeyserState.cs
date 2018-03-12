using UnityEngine;

public class GeyserState : BossState<GeyserPattern>
{
    // Here place some values to tweak.
    public float delayAfterGeyserChase = 2f;
    public float timeBeforeSpawning = 2f;

    protected override GeyserPattern Init()
    {
        return new GeyserPattern(this);
	}
}
