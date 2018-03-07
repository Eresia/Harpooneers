using System.Collections;
using System.Collections.Generic;
using Rewired;
using UnityEngine;

public class InputInMainMenu : MonoBehaviour {
    
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
                if(!playerHasJoined[i])
                {
                    playerHasJoined[i] = true;
                    nbOfPlayers++;
                }

                // TODO Call join on the customization manager
            }

            else if (players[i].GetButtonDown("Cancel"))
            {
                if(playerReady[i])
                {
                    nbOfPlayersReady--;
                    playerReady[i] = false;

                    // TODO Call Not ready i on the customization manager
                }

                else
                {
                    if(playerHasJoined[i])
                    {
                        playerHasJoined[i] = false;
                        nbOfPlayers--;
                    }
                    // TODO Call Leave i on the customization manager
                }
            }

            else if (players[i].GetButtonDown("Start"))
            {
                if(playerReady[i])
                {
                    // Player 1 start the game if all players are ready !
                    if (i == 0)
                    {
                        // All players are ready.
                        if (nbOfPlayers == nbOfPlayersReady)
                        {
                            GameManager.instance.StartNewGame(nbOfPlayers);
                        }
                    }
                }

                else
                {
                    playerReady[i] = true;
                    nbOfPlayersReady++;

                    // TODO Call Ready i on the customization manager
                }
            }
        }
    }
}
