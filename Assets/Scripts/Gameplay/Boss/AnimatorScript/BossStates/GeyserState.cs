using UnityEngine;

public class GeyserState : BossState<GeyserPattern>
{
    public float delayAfterGeyserChase = 2f;

    [Header("Emergence")]
    public float emergingDuration = 5f;
    
    [Tooltip("Don't touch the x value !")]
    public Vector3 emergePosStart = new Vector3(0f, -20f, -10f);

    [Tooltip("Don't touch the x value !")]
    public Vector3 emergePosEnd = new Vector3(0f, 1.0f, 8f);

    public Vector3 emergeRotationStart;
    public Vector3 emergeRotationEnd;

    [Space(10)]

    public float waitDuration = 10f;

    [Header("Dive")]
    public float divingDuration = 5f;
    public float diveHeightEnd = -30f;
    public float diveForwardEnd = 20f;

    public Vector3 diveRotationEnd;

    protected override GeyserPattern Init()
    {
        return new GeyserPattern(this);
	}
}
