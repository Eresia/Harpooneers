using UnityEngine;
using System.Collections;
using UnityEditor;

//This class helps us make scriptable objects
public class CreateScriptableObject
{
    [MenuItem("Assets/Create/ShipModules/Harpoon")]
    public static void CreateHarpoonModule()
    {
        HarpoonModule module = ScriptableObject.CreateInstance<HarpoonModule>();
        AssetDatabase.CreateAsset(module, "Assets/Scripts/Gameplay/ShipModules/ScriptableObjects/harpoon.asset");
        AssetDatabase.SaveAssets();
    }
}
