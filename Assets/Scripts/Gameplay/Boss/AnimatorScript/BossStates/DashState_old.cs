﻿using UnityEngine;

public class DashState_old : BossState<DashPattern_old>
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

    private DashPattern_old dashPattern;

    [HideInInspector]
    public float minX, maxX, minZ, maxZ;

	protected override DashPattern_old Init()
    {
        minX = Mathf.Lerp(GameManager.instance.boundaryMgr.trapezeData.xBottomLeft, GameManager.instance.boundaryMgr.trapezeData.xBottomRight, 0.5f - offsetX);
        maxX = Mathf.Lerp(GameManager.instance.boundaryMgr.trapezeData.xBottomLeft, GameManager.instance.boundaryMgr.trapezeData.xBottomRight, 0.5f + offsetX);
        minZ = Mathf.Lerp(GameManager.instance.boundaryMgr.trapezeData.zBottom, GameManager.instance.boundaryMgr.trapezeData.zTop, 0.5f - offsetY);
        maxZ = Mathf.Lerp(GameManager.instance.boundaryMgr.trapezeData.zBottom, GameManager.instance.boundaryMgr.trapezeData.zTop, 0.5f + offsetY);

        return new DashPattern_old(this);
	}
}
