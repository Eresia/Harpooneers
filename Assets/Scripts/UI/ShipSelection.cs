using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipSelection : MonoBehaviour {

    public int playerID;
    public int currentHarpoonID;
    public int currentCoqueID;
    public int currentCabinID;
    public int currentBombID;

    public Material[] shipMaterials;

    public GameObject[] harpoonsGoArray;
    public GameObject[] coquesGoArray;
    public GameObject[] cabinsGoArray;
    public GameObject[] bombsGoArray;

    [Space(20)]
    public Text[] modulesNameFields;
    public Text[]moduleDescriptionFields;

    private ShipConfiguration currentConfig;
    public int currentTabID;    

    void Awake()
    {
        foreach(GameObject go in harpoonsGoArray)
        {
            go.GetComponent<MeshRenderer>().material = shipMaterials[playerID];
        }
        foreach (GameObject go in coquesGoArray)
        {
            go.GetComponent<MeshRenderer>().material = shipMaterials[playerID];
        }
        foreach (GameObject go in cabinsGoArray)
        {
            go.GetComponent<MeshRenderer>().material = shipMaterials[playerID];
        }
        foreach (GameObject go in bombsGoArray)
        {
            go.GetComponent<MeshRenderer>().material = shipMaterials[playerID];
        }
    }


    // Use this for initialization
    void Start () {
        RandomizeShip();
    }

    public void PreviousModule()
    {
        switch(currentTabID)
        {
            case 0:
                currentHarpoonID--;
                if(currentHarpoonID < 0)
                   currentHarpoonID = GameManager.instance.shipMgr.harpoonsScriptObjs.Length-1;
                UpdateHarpoon();
                break;

            case 1:
                currentCoqueID--;
                if (currentCoqueID < 0)
                    currentCoqueID = GameManager.instance.shipMgr.coquesScriptObjs.Length - 1;
                UpdateCoque();
                break;

            case 2:
                currentCabinID--;
                if (currentCabinID < 0)
                    currentCabinID = GameManager.instance.shipMgr.cabinesScriptObjs.Length - 1;
                UpdateCabin();
                break;

            case 3:
                currentBombID--;
                if(currentBombID < 0)
                   currentBombID = GameManager.instance.shipMgr.bombsScriptObjs.Length - 1;
                UpdateBomb();
                break;
        }
     
    }

    public void NextModule()
    {
        switch (currentTabID)
        {
            case 0:
                currentHarpoonID++;
                if (currentHarpoonID > GameManager.instance.shipMgr.harpoonsScriptObjs.Length - 1)
                    currentHarpoonID = 0;
                UpdateHarpoon();
                break;

            case 1:
                currentCoqueID++;
                if (currentCoqueID > GameManager.instance.shipMgr.coquesScriptObjs.Length - 1)
                    currentCoqueID = 0;
                UpdateCoque();
                break;

            case 2:
                currentCabinID++;
                if (currentCabinID > GameManager.instance.shipMgr.cabinesScriptObjs.Length - 1)
                    currentCabinID = 0;
                UpdateCabin();
                break;

            case 3:
                currentBombID++;
                if (currentBombID > GameManager.instance.shipMgr.bombsScriptObjs.Length - 1)
                    currentBombID = 0;
                UpdateBomb();
                break;
        }

    }

    public void UpdateHarpoon()
    {
        for (int i = 0; i < harpoonsGoArray.Length; i++)
        {
            harpoonsGoArray[i].SetActive(false);
        }
        harpoonsGoArray[currentHarpoonID].SetActive(true);

        modulesNameFields[0].text = GameManager.instance.shipMgr.harpoonsScriptObjs[currentHarpoonID].moduleName;
        moduleDescriptionFields[0].text = GameManager.instance.shipMgr.harpoonsScriptObjs[currentHarpoonID].description;
    }

    public void UpdateCoque()
    {
        for (int i = 0; i < coquesGoArray.Length; i++)
        {
            coquesGoArray[i].SetActive(false);
        }
        coquesGoArray[currentCoqueID].SetActive(true);

        modulesNameFields[1].text = GameManager.instance.shipMgr.coquesScriptObjs[currentCoqueID].moduleName;
        moduleDescriptionFields[1].text = GameManager.instance.shipMgr.coquesScriptObjs[currentCoqueID].description;
    }

    public void UpdateCabin()
    {
        for (int i = 0; i < cabinsGoArray.Length; i++)
        {
            cabinsGoArray[i].SetActive(false);
        }
        cabinsGoArray[currentCabinID].SetActive(true);

        modulesNameFields[2].text = GameManager.instance.shipMgr.cabinesScriptObjs[currentCabinID].moduleName;
        moduleDescriptionFields[2].text = GameManager.instance.shipMgr.cabinesScriptObjs[currentCabinID].description;
    }

    public void UpdateBomb()
    {
        for (int i = 0; i < bombsGoArray.Length; i++)
        {
            bombsGoArray[i].SetActive(false);
        }
        bombsGoArray[currentBombID].SetActive(true);

        modulesNameFields[3].text = GameManager.instance.shipMgr.bombsScriptObjs[currentBombID].moduleName;
        moduleDescriptionFields[3].text = GameManager.instance.shipMgr.bombsScriptObjs[currentBombID].description;
    }

    public void RandomizeShip()
    {
        currentHarpoonID = Random.Range(0, GameManager.instance.shipMgr.harpoonsScriptObjs.Length);
        UpdateHarpoon();

        currentCoqueID = Random.Range(0, GameManager.instance.shipMgr.coquesScriptObjs.Length);
        UpdateCoque();

        currentCabinID = Random.Range(0, GameManager.instance.shipMgr.cabinesScriptObjs.Length);
        UpdateCabin();

        currentBombID = Random.Range(0, GameManager.instance.shipMgr.bombsScriptObjs.Length);
        UpdateBomb();

        UpdateConfig();
    }

    // Update the config in the game manager.
    private void UpdateConfig()
    {
        currentConfig.harpoonId = currentHarpoonID;
        currentConfig.coqueId = currentCoqueID;
        currentConfig.cabinId = currentCabinID;
        currentConfig.bombStockId = currentBombID;

        GameManager.instance.shipConfigs[playerID] = currentConfig;
    }
}
