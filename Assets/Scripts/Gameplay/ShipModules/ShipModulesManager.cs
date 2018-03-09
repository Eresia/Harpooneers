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

    [Header("Components scripts")]
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

    private void FixedUpdate()
    {
        // TODO REMOVE when tweaking is finished.

        if(GameManager.instance.shipMgr.tweaking)
        {
            _movementScript.physicMove.friction = _movementScript.coqueModule.drag;
            _movementScript.physicMove.gravity = _movementScript.coqueModule.waveResistance;
            
            _bombScript.physicsScript.gravity = _bombScript.bombStockModule.bombWavesResistance;
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

            // Configure coque and impact move physic.
            _movementScript.coqueModule = shipMgr.coquesScriptObjs[config.coqueId];
            _movementScript.physicMove.limitSpeed = _movementScript.coqueModule.moveSpeedMax;
            _movementScript.physicMove.friction = _movementScript.coqueModule.drag;
            _movementScript.physicMove.gravity = _movementScript.coqueModule.waveResistance;

            _bombScript.bombStockModule = shipMgr.bombsScriptObjs[config.bombStockId];
            _bombScript.physicsScript.gravity = _bombScript.bombStockModule.bombWavesResistance;

            _bombScript.SetupBombFX();
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
