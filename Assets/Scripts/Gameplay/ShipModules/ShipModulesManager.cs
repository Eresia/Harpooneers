using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage modules of a ship (Setup / Reset).
/// </summary>
public class ShipModulesManager : MonoBehaviour {

    [Header("Ship elements")]
    public GameObject[] harpoonsGoArray;
    public GameObject[] coquesGoArray;
    public GameObject[] cabinsGoArray;
    public GameObject[] bombsGoArray;

    private HarpoonLauncher _harpoonScript;
    private MovementBehaviour _movementScript;

    private PlayerInput _inputScript;

	// Use this for initialization
	void Awake ()
    {
        _harpoonScript = GetComponent<HarpoonLauncher>();
        _movementScript = GetComponent<MovementBehaviour>();
        _inputScript = GetComponent<PlayerInput>();
    }
    
    void Start()
    {
        // Default config for debug.
        ShipConfiguration config = GameManager.instance.defaultConfig;
        ShipManager moduleMgr = FindObjectOfType<ShipManager>();

        ActivateShipModules(config, moduleMgr);
    }

    public void ActivateShipModules(ShipConfiguration config, ShipManager moduleMgr)
    {
        ResetModules();

        // Setup the boat model.
        harpoonsGoArray[config.harpoonId].SetActive(true);
        coquesGoArray[config.coqueId].SetActive(true);
        cabinsGoArray[config.cabinId].SetActive(true);
        bombsGoArray[config.bombStockId].SetActive(true);
        
        // Setup the data of each modules.
        //_harpoonScript.harpoonModule = moduleMgr.harpoonsScriptObjs[config.harpoonId];
        //_movementScript.coqueModule = moduleMgr.coquesScriptObjs[config.coqueId];
        //_bombScript.bombStockModule = moduleMgr.bombsScriptObjs[config.bombStockId];
    }

    /// <summary>
    /// Reset modules on the boat.
    /// </summary>
    public void ResetModules()
    {
        for (int i = 0; i < harpoonsGoArray.Length; i++)
        {
            harpoonsGoArray[i].SetActive(false);
        }

        for (int i = 0; i < coquesGoArray.Length; i++)
        {
            coquesGoArray[i].SetActive(false);
        }

        for (int i = 0; i < cabinsGoArray.Length; i++)
        {
            cabinsGoArray[i].SetActive(false);
        }

        for (int i = 0; i < bombsGoArray.Length; i++)
        {
            bombsGoArray[i].SetActive(false);
        }
    }
}
