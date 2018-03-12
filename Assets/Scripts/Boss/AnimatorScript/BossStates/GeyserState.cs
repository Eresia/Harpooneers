using UnityEngine;

public class GeyserState : BossState<GeyserPattern>
{
    // Here place some values to tweak.

	protected override GeyserPattern Init()
    {
        return new GeyserPattern(this);
	}
}
