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

[System.Serializable]
public class BombModule : ScriptableObject
{

    public string moduleName;
    public int moduleId;
    public string description;

    public float bombRadius;
    public float bombWavesResistance;
}

[System.Serializable]
public class CoqueModule : ScriptableObject
{

    public string moduleName;
    public int moduleId;
    public string description;

    public float turnRate;
    public float moveSpeed;
    public float hitboxSize;
}

[System.Serializable]
public class CabineModule : ScriptableObject
{

    public string moduleName;
    public int moduleId;
    public string description;

   // The cabin is purely cosmetic
}
