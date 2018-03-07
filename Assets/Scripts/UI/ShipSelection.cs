using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Use this for initialization
    void Start () {
		
	}

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("mouse here");
        }

        if (Input.GetKey(KeyCode.A))
          {
                Debug.Log("keyboard here");
          }
        }

    public void UpdateHarpoon()
    {
        for (int i = 0; i < harpoonsGoArray.Length; i++)
        {
            harpoonsGoArray[i].SetActive(false);
        }
        harpoonsGoArray[currentHarpoonID].SetActive(true);
    }

    public void UpdateCoque()
    {
        for (int i = 0; i < coquesGoArray.Length; i++)
        {
            coquesGoArray[i].SetActive(false);
        }
        coquesGoArray[currentCoqueID].SetActive(true);
    }

    public void UpdateCabin()
    {
        for (int i = 0; i < cabinsGoArray.Length; i++)
        {
            cabinsGoArray[i].SetActive(false);
        }
        cabinsGoArray[currentCabinID].SetActive(true);
    }

    public void UpdateBomb()
    {
        for (int i = 0; i < bombsGoArray.Length; i++)
        {
            bombsGoArray[i].SetActive(false);
        }
        bombsGoArray[currentBombID].SetActive(true);
    }

    public void RandomizeShip()
    {
        Debug.Log("RANDOOOM");

        currentHarpoonID = Random.Range(0, 3);
        UpdateHarpoon();

        currentCoqueID = Random.Range(0, 3);
        UpdateCoque();

        currentCabinID = Random.Range(0, 3);
        UpdateCabin();

        currentBombID = Random.Range(0, 3);
        UpdateBomb();
    }
}
