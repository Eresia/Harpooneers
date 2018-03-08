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
        AssetDatabase.CreateAsset(module, "Assets/ScriptableObjects/Harpoon.asset");
        AssetDatabase.SaveAssets();
        
        EditorGUIUtility.PingObject(module);
    }

    [MenuItem("Assets/Create/ShipModules/BombStock")]
    public static void CreateBombStockModule()
    {
        BombStockModule module = ScriptableObject.CreateInstance<BombStockModule>();
        AssetDatabase.CreateAsset(module, "Assets/ScriptableObjects/BombStock.asset");
        AssetDatabase.SaveAssets();
        
        EditorGUIUtility.PingObject(module);
    }

    [MenuItem("Assets/Create/ShipModules/Coque")]
    public static void CreateCoqueModule()
    {
        CoqueModule module = ScriptableObject.CreateInstance<CoqueModule>();
        AssetDatabase.CreateAsset(module, "Assets/ScriptableObjects/Coque.asset");
        AssetDatabase.SaveAssets();

        EditorGUIUtility.PingObject(module);
    }

    [MenuItem("Assets/Create/ShipModules/Cabine")]
    public static void CreateCabineModule()
    {
        CabineModule module = ScriptableObject.CreateInstance<CabineModule>();
        AssetDatabase.CreateAsset(module, "Assets/ScriptableObjects/Cabine.asset");
        AssetDatabase.SaveAssets();

        EditorGUIUtility.PingObject(module);
    }
}
