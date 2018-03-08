using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class InputInMainMenu : MonoBehaviour {

    public PrepareGamePanelScript prepareGamePanel;

    private Player[] players;

    private bool[] playerHasJoined;
    private bool[] playerReady;

    public int nbOfPlayers;
    public int nbOfPlayersReady;

    private void Awake()
    {
        playerReady = new bool[4];
        playerHasJoined = new bool[4];
        players = new Player[4];

        for (int i = 0; i < 4; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);
        }
    }

    private void Update()
    {
        for (int i = 0; i < players.Length; i++)
        {
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
                else
                {
                    playerReady[i] = true;
                    nbOfPlayersReady++;

                    prepareGamePanel.PlayerReady(i);
                }
            }
        }
    }
}
