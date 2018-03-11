using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BombStockModule : ScriptableObject
{
    public string moduleName;
    public int moduleId;
    public string description;

    public float bombRadius;
    public float bombWavesResistance;
}
