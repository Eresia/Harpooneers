using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CoqueModule : ScriptableObject
{
    public string moduleName;
    public int moduleId;
    public string description;

    public float moveSpeed;
    public float maxSpeed;
    public float turnSpeed;
    public float hitboxSize;
}
