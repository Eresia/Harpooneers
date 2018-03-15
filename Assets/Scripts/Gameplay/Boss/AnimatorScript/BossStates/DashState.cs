using UnityEngine;

public class DashState : BossState<DashPattern>
{    
	public float depth = 2f;
    public float timeinSea = 5f;
    
    public float translateSpeed = 20f;

	public float ySpeed = 2f;

	public float turnSpeed = 1f;

	public float targetChangeTime = 5f;

	public float maxTime = 20f;

	public float endOfPaternTime = 2f;

	public bool test;
	public float test2;

    private DashPattern dashPattern;

	protected override DashPattern Init()
    {
        return new DashPattern(this);
	}
}
