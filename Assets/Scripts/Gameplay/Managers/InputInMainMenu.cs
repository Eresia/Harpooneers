using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[System.Serializable]
public class InputList
{
    public Button[] inputs;
}

[System.Serializable]
public class TabsList
{
    public Image[] tabs;
    public GameObject[] moduleSelectionGO;
}


public class InputInMainMenu : MonoBehaviour {

    public PrepareGamePanelScript prepareGamePanel;

    private Player[] players;

    private bool[] playerHasJoined;
    private bool[] playerReady;

    public int nbOfPlayers;
    public int nbOfPlayersReady;

    public ShipSelection[] playerShips;
  
    private int[] currentModuleTabIndex;

    public TabsList[] playerTabs;
    public InputList[] playerInputs;

    private PointerEventData pointer;

    private bool[] _isAxisInUse;
    private float[] _timeSinceJoystick;
    public float timeBetweenJoystickInput;

    private void Awake()
    {
        playerReady = new bool[4];
        playerHasJoined = new bool[4];
        players = new Player[4];
        _isAxisInUse = new bool[4];
        _timeSinceJoystick = new float[4];

        for (int i = 0; i < 4; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);
            playerShips[i].playerID = i;
        }

     
        currentModuleTabIndex = new int[4];

        for (int i = 0; i < currentModuleTabIndex.Length; i++)
        {
            currentModuleTabIndex[i] = 0;
        }

      
    }

    void Start()
    {
        for (int i = 0; i < players.Length; i++)
        {
            NextModuleTab(i, 0);
        }
    }

    private void Update()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetAxisRaw("Module - L Joystick") < 0)
            {
                if (_isAxisInUse[i] == false)
                {
                    pointer = new PointerEventData(EventSystem.current);
                    ExecuteEvents.Execute(playerInputs[i].inputs[0].gameObject, pointer, ExecuteEvents.submitHandler);
                    _isAxisInUse[i] = true;
                }
            }

            if (players[i].GetAxisRaw("Module - L Joystick") > 0)
            {
                if (_isAxisInUse[i] == false)
                {
                    pointer = new PointerEventData(EventSystem.current);
                    ExecuteEvents.Execute(playerInputs[i].inputs[1].gameObject, pointer, ExecuteEvents.submitHandler);
                    _isAxisInUse[i] = true;
                }
            }

            if (players[i].GetAxisRaw("Module - L Joystick") == 0)
            {
                _isAxisInUse[i] = false;
            }

            if(_isAxisInUse[i] == true)
            {
                _timeSinceJoystick[i] += Time.deltaTime;
            }
            else
            {
                _timeSinceJoystick[i] = 0;
            }

            if(_timeSinceJoystick[i] >= timeBetweenJoystickInput)
            {
                _isAxisInUse[i] = false;
                _timeSinceJoystick[i] = 0;
            }
            

            if (players[i].GetButtonDown("Submit"))
            {
                // Player joins the game.
                if(!playerHasJoined[i])
                {
                    playerHasJoined[i] = true;
                    nbOfPlayers++;
                    
                    prepareGamePanel.PlayerJoin(i);
                }
            }

            else if (players[i].GetButtonDown("Cancel"))
            {
                // Player is not ready anymore.
                if(playerReady[i])
                {
                    nbOfPlayersReady--;
                    playerReady[i] = false;
                    
                    prepareGamePanel.PlayerNotReady(i);
                }

                // Player left the pool of players.
                else
                {
                    if(playerHasJoined[i])
                    {
                        playerHasJoined[i] = false;
                        nbOfPlayers--;

                        prepareGamePanel.PlayerLeave(i);
                    }
                }
            }

            else if (players[i].GetButtonDown("Start"))
            {
                if (playerReady[i])
                {
                    // Player 1 start the game if all players are ready !
                    if (i == 0)
                    {
                        // All players are ready and number of players are minimum 2 !
                        if (nbOfPlayers > 1 && nbOfPlayers == nbOfPlayersReady)
                        {
                            GameManager.instance.StartNewGame(nbOfPlayers, playerReady);
                        }
                    }
                }

                // Player is now ready.
                else if (playerHasJoined[i])
                {
                    playerReady[i] = true;
                    nbOfPlayersReady++;

                    prepareGamePanel.PlayerReady(i);
                }
            }

            // Change module family
            if (players[i].GetButtonDown("PreviousTab"))
            {
                NextModuleTab(i, -1);
            }
            if (players[i].GetButtonDown("NextTab"))
            {
                NextModuleTab(i, 1);
            }


            // Change module
            if (players[i].GetButtonDown("PreviousModule"))
            {
                pointer = new PointerEventData(EventSystem.current);
                ExecuteEvents.Execute(playerInputs[i].inputs[0].gameObject, pointer, ExecuteEvents.submitHandler);
            }

            if (players[i].GetButtonDown("NextModule"))
            {
                pointer = new PointerEventData(EventSystem.current);
                ExecuteEvents.Execute(playerInputs[i].inputs[1].gameObject, pointer, ExecuteEvents.submitHandler);
            }
            if (players[i].GetButtonDown("RandomShip"))
            {
                pointer = new PointerEventData(EventSystem.current);
                ExecuteEvents.Execute(playerInputs[i].inputs[2].gameObject, pointer, ExecuteEvents.submitHandler);
            }
        }
    }

    private void NextModuleTab(int playerID, int direction)
    {   
        currentModuleTabIndex[playerID] += direction;
        if (currentModuleTabIndex[playerID] < 0)
            currentModuleTabIndex[playerID] = playerTabs[playerID].moduleSelectionGO.Length - 1;
        currentModuleTabIndex[playerID] = currentModuleTabIndex[playerID] % playerTabs[playerID].moduleSelectionGO.Length;

        for (int i = 0; i < playerTabs[playerID].moduleSelectionGO.Length; i++)
        {
            playerTabs[playerID].moduleSelectionGO[i].SetActive(false);
            playerTabs[playerID].tabs[i].color = Color.gray;
        }
        playerShips[playerID].currentTabID = currentModuleTabIndex[playerID];
        playerTabs[playerID].moduleSelectionGO[currentModuleTabIndex[playerID]].SetActive(true);
        playerTabs[playerID].tabs[currentModuleTabIndex[playerID]].color = Color.white;      
    }
}