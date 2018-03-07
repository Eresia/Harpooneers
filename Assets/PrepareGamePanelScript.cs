using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrepareGamePanelScript : MonoBehaviour {
    
    public GameObject[] joinPanel;
    public GameObject[] shipPanel;
    public GameObject[] readyPanel;
    public GameObject[] notReadyPanel;

    private void Awake()
    {
        for (int i = 0; i < 4; i++)
        {
            PlayerLeave(i);
            PlayerNotReady(i);
        }
    }

    public void PlayerJoin(int playerId)
    {
        joinPanel[playerId].SetActive(false);
        shipPanel[playerId].SetActive(true);
    }

    public void PlayerLeave(int playerId)
    {
        joinPanel[playerId].SetActive(true);
        shipPanel[playerId].SetActive(false);
    }

    public void PlayerReady(int playerId)
    {
        notReadyPanel[playerId].SetActive(false);
        readyPanel[playerId].SetActive(true);
    }

    public void PlayerNotReady(int playerId)
    {
        notReadyPanel[playerId].SetActive(true);
        readyPanel[playerId].SetActive(false);
    }
}

