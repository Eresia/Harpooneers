using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CoqueModule : ScriptableObject
{
    public string moduleName = "La bonne Coque";
    public int moduleId;
    public string description = "A nice hull";
    
    public float moveSpeedMax = 0.005f;
    public float turnSpeed = 5;

    public float hitboxSize;

    public float drag = 2;
    public float waveSensibility = 20;
}
