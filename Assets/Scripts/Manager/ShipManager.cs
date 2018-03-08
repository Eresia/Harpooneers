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
    public CabineModule[] cabinesScriptObjs;
    public BombStockModule[] bombsScriptObjs;

    public void SetupAllShips()
    {
        for (int i = 0; i < shipModuleMgrs.Length; i++)
        {
            // Check if a player is in the game.
            if(GameManager.instance.players[i])
            {
                Debug.Log("Player " + i + " active !");

                // For debug only. Load default config.
                if(GameManager.instance.debug)
                {
                    shipModuleMgrs[i].ActivateShipModules(GameManager.instance.defaultConfig, this);
                }

                else
                {
                    shipModuleMgrs[i].ActivateShipModules(GameManager.instance.shipConfigs[i], this);
                }
                
                ships[i].SetActive(true);
            }

            else
            {
                Debug.Log("Player " + i + " inactive !");
                ships[i].SetActive(false);
            }
        }
    }
}
