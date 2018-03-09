using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CoqueModule : ScriptableObject
{
    public string moduleName;
    public int moduleId;
    public string description;
    
    public float moveSpeedMax;
    public float turnSpeed;

    public float hitboxSize;

    public float drag = 1;
    public float waveResistance = 5;
}
