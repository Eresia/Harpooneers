using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class ShipNameGenerator : MonoBehaviour
{
    public ShipSelection shipSelect;
    public TextMeshProUGUI shipName;
    public TextMeshProUGUI[] moduleNames;

    public string filePath;

    private List<string> shipNamesList = new List<string>();

  //  private byte shipSetupByte;
    private int shipSetupID;

    void Awake()
    {
        ReadTextFile();
    }

    void OnEnable()
    {
        // Summary of modules
        for(int i = 0; i < 3; i++)
             moduleNames[i].text = shipSelect.modulesNameFields[i].text;


        int id = CalculateSetupID();
        if(id <= shipNamesList.Count)
        {
            shipName.text = "'" + shipNamesList[id] + "'";
        }
        else
        {
            shipName.text = "'Error in Name'";
        }   
    }

    void ReadTextFile()
    {
        StreamReader inp_stm = new StreamReader(filePath);

        while (!inp_stm.EndOfStream)
        {
            string inp_ln = inp_stm.ReadLine();
            shipNamesList.Add(inp_ln);
        }

        inp_stm.Close();
    }

    private int CalculateSetupID()
    {
        shipSetupID = shipSelect.currentHarpoonID;
        shipSetupID += shipSelect.currentCoqueID * 4;
        shipSetupID += shipSelect.currentCabinID * 16;
        shipSetupID += shipSelect.currentBombID * 64;
        Debug.Log(shipSetupID);
        return shipSetupID;
    }
}
	