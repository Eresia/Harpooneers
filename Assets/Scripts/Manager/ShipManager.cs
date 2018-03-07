using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Setup all boats in the game depending the number of players and the customizations choosen.
/// </summary>
public class ShipManager : MonoBehaviour {
    
    public GameObject[] ships;
    public ShipModulesManager[] shipModuleMgrs;

    [Header("Modules for boat (Scriptable objects)")]
    public HarpoonModule[] harpoonsScriptObjs;
    public CoqueModule[] coquesScriptObjs;
    public BombStockModule[] bombsScriptObjs;
    public CabineModule[] cabinesScriptObjs;


    // Use this for initialization
    void Start () {
        //SetupAllShips();
	}
    
    private void SetupAllShips()
    {
        for (int i = 0; i < shipModuleMgrs.Length; i++)
        {
            if(i < GameManager.instance.nbOfPlayers)
            {
                shipModuleMgrs[i].ActivateShipModules(GameManager.instance.shipConfigs[i], this);
                ships[i].SetActive(true);
            }

            else
            {
                ships[i].SetActive(false);
            }
        }
    }
}
