using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShipModulesManager : MonoBehaviour {

    public GameObject[] harpoonsGoArray;
    public GameObject[] coquesGoArray;
    public GameObject[] cabinesGoArray;
    public GameObject[] bombsGoArray;

    public HarpoonModule[] harpoonsScriptObjs;
    public CoqueModule[] coquesScriptObjs;
    public BombModule[] bombsScriptObjs;
    public CabineModule[] cabinesScriptObjs;

    private HarpoonLauncher _harpoonScript;
    private MovementBehaviour _movementScript;
    private PlayerInput _inputScript;
    private ExplosiveBarrel _bombScript;

	// Use this for initialization
	void Awake ()
    {
        _harpoonScript = GetComponent<HarpoonLauncher>();
        _movementScript = GetComponent<MovementBehaviour>();
        _inputScript = GetComponent<PlayerInput>();
        _bombScript = _inputScript.playerBomb;
    }

    void Start()
    {
        ActivateShipModules(0, 2, 1, 2);
    }

    public void ActivateShipModules(int harpoonID, int coqueID, int cabineID, int bombID)
    {
        DeactivateAllModules();

        harpoonsGoArray[harpoonID].SetActive(true);
        coquesGoArray[coqueID].SetActive(true);
        cabinesGoArray[cabineID].SetActive(true);
        bombsGoArray[bombID].SetActive(true);


        // Update Harpoon parameters
        for (int i = 0; i < harpoonsScriptObjs.Length; i++)
        {
            if (harpoonsScriptObjs[i].moduleId == harpoonID)
            {
                //TODO send harpoonsScriptObjs[i] to _harpoonScript
            }
        }

        // Update Bombs parameters
        for (int i = 0; i < bombsScriptObjs.Length; i++)
        {
            if (bombsScriptObjs[i].moduleId == harpoonID)
            {
                //TODO send bombsScriptObjs[i] to _bombScript
            }
        }

        // Update Coque parameters
        for (int i = 0; i < coquesScriptObjs.Length; i++)
        {
            if (coquesScriptObjs[i].moduleId == harpoonID)
            {
                //TODO send coquesScriptObjs[i] to _movementScript
            }
        }

        // Update Cabine parameters
        for (int i = 0; i < cabinesScriptObjs.Length; i++)
        {
            if (cabinesScriptObjs[i].moduleId == harpoonID)
            {
                //TODO send cabinesScriptObjs[i] to nothing, because it's cosmetic. Otherwise it's here.
            }
        }


    }

    public void RandomShipModules()
    {
        
    }

    public void DeactivateAllModules()
    {
        for (int i = 0; i < harpoonsGoArray.Length; i++)
        {
            harpoonsGoArray[i].SetActive(false);
        }

        for (int i = 0; i < coquesGoArray.Length; i++)
        {
            coquesGoArray[i].SetActive(false);
        }

        for (int i = 0; i < cabinesGoArray.Length; i++)
        {
            cabinesGoArray[i].SetActive(false);
        }

        for (int i = 0; i < bombsGoArray.Length; i++)
        {
            bombsGoArray[i].SetActive(false);
        }
    }
}
