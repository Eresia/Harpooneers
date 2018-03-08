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

    public GameObject[] harpoonsGoArray;
    public GameObject[] coquesGoArray;
    public GameObject[] cabinsGoArray;
    public GameObject[] bombsGoArray;

    [Space(20)]
    public Text[] modulesNameFields;
    public Text[]moduleDescriptionFields;

    private ShipConfiguration currentConfig;
    

    // Use this for initialization
    void Start () {
        RandomizeShip();

       // GameManager.instance.shipMgr.harpoonsScriptObjs[currentHarpoonID].name;
    }

    public void PreviousModule(int id)
    {
        switch(id)
        {
            case 0:
                currentHarpoonID--;
                if(currentHarpoonID < 0)
                   currentHarpoonID = 2;
                UpdateHarpoon();
                break;

            case 1:
                currentCoqueID--;
                if (currentCoqueID < 0)
                    currentCoqueID = 2;
                UpdateCoque();
                break;

            case 2:
                currentCabinID--;
                if (currentCabinID < 0)
                    currentCabinID = 2;
                UpdateCabin();
                break;

            case 3:
                currentBombID--;
                if(currentBombID < 0)
                   currentBombID = 2;
                UpdateBomb();
                break;
        }
     
    }

    public void NextModule(int id)
    {
        switch (id)
        {
            case 0:
                currentHarpoonID++;
                if (currentHarpoonID > 2)
                    currentHarpoonID = 0;
                UpdateHarpoon();
                break;

            case 1:
                currentCoqueID++;
                if (currentCoqueID > 2)
                    currentCoqueID = 0;
                UpdateCoque();
                break;

            case 2:
                currentCabinID++;
                if (currentCabinID > 2)
                    currentCabinID = 0;
                UpdateCabin();
                break;

            case 3:
                currentBombID++;
                if (currentBombID > 2)
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
        currentHarpoonID = Random.Range(0, 3);
        UpdateHarpoon();

        currentCoqueID = Random.Range(0, 3);
        UpdateCoque();

        currentCabinID = Random.Range(0, 3);
        UpdateCabin();

        currentBombID = Random.Range(0, 3);
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
