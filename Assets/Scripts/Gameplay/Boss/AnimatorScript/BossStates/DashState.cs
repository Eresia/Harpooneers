using UnityEngine;

public class DashState : BossState<DashPattern>
{
    [Header("Distance with spawn (screen offset)")]
    public float offsetX = 0.3f;
    public float offsetY = 0.1f;

    [Header("Preview")]
    public int TurnMin = 3;
    public int TurnMax = 5;
    
    [Header("Effect duration")]
    [Tooltip("Bubble duration before spawning")]
    public float WaitBeforeSpawn = 2f;
    public float WaitBeforeDash = 0.5f;
    
    public float translateSpeed = 20f;
    public float dashSpeed = 90f;
    public float whaleOffset = 18f;
    
    public bool diveOnExplode = true;

    [Header("Dive")]
    public float divingDuration = 5f;
    public float diveHeightEnd = -30f;
    public float diveForwardEnd = 20f;

    public Vector3 diveRotationEnd;

    private DashPattern dashPattern;

    [HideInInspector]
    public float minX, maxX, minZ, maxZ;

	protected override DashPattern Init()
    {
        minX = Mathf.Lerp(GameManager.instance.boundaryMgr.trapezeData.xBottomLeft, GameManager.instance.boundaryMgr.trapezeData.xBottomRight, 0.5f - offsetX);
        maxX = Mathf.Lerp(GameManager.instance.boundaryMgr.trapezeData.xBottomLeft, GameManager.instance.boundaryMgr.trapezeData.xBottomRight, 0.5f + offsetX);
        minZ = Mathf.Lerp(GameManager.instance.boundaryMgr.trapezeData.zBottom, GameManager.instance.boundaryMgr.trapezeData.zTop, 0.5f - offsetY);
        maxZ = Mathf.Lerp(GameManager.instance.boundaryMgr.trapezeData.zBottom, GameManager.instance.boundaryMgr.trapezeData.zTop, 0.5f + offsetY);

        return new DashPattern(this);
	}
}
