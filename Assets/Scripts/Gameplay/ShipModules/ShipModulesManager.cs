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

    public HarpoonLauncher _harpoonScript;
    public MovementBehaviour _movementScript;
    public ExplosiveBarrel _bombScript;

	// Use this for initialization
	void Reset ()
    {
        _harpoonScript = GetComponent<HarpoonLauncher>();
        _movementScript = GetComponent<MovementBehaviour>();
        _bombScript = transform.parent.GetComponentInChildren<ExplosiveBarrel>();
    }
    
    void Start()
    {
        if(GameManager.instance.debug)
        {
            // Default config for debug.
            ShipConfiguration config = GameManager.instance.defaultConfig;
            ShipManager shipMgr = FindObjectOfType<ShipManager>();
            
            ActivateShipModules(config, shipMgr);
        }
    }

    public void ActivateShipModules(ShipConfiguration config, ShipManager shipMgr)
    {
        ResetModules();

        // Setup the boat model.
        harpoonsGoArray[config.harpoonId].SetActive(true);
        coquesGoArray[config.coqueId].SetActive(true);
        cabinsGoArray[config.cabinId].SetActive(true);
        bombsGoArray[config.bombStockId].SetActive(true);

        if (shipMgr)
        {
            // Setup the data of each modules.
            _harpoonScript.harpoonModule = shipMgr.harpoonsScriptObjs[config.harpoonId];
            _movementScript.coqueModule = shipMgr.coquesScriptObjs[config.coqueId];
            //_movementScript.physicMove.limitSpeed = _movementScript.coqueModule.maxSpeed;
            _bombScript.bombStockModule = shipMgr.bombsScriptObjs[config.bombStockId];
        }
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
