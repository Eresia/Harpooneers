using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HarpoonModule : ScriptableObject {

    public string moduleName;
    public int moduleId;
    public string description;

    public float retractSpeed;
    public float fireSpeed;
    public float fireDistance;
}
