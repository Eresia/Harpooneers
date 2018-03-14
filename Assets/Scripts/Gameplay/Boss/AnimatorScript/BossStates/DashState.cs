using UnityEngine;

public class DashState : BossState<DashPattern>
{    
    public float timeinSea = 2f;
    
    public float translateSpeed = 20f;

	public float turnSpeed = 1f;

	public float targetChangeTime = 5f;

    private DashPattern dashPattern;

	protected override DashPattern Init()
    {
        return new DashPattern(this);
	}
}
