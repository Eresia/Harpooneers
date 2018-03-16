using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Rewired;

public class Pause : MonoBehaviour {

    public GameObject pauseGo;

    private Player pausePlayer;

    void Start()
    {
        pauseGo.SetActive(false);
    }

    public void PauseGame(int playerID)
    {
        pauseGo.SetActive(true);

        pausePlayer = ReInput.players.GetPlayer(playerID);

        Time.timeScale = 0f;
    }

    private void Update()
    {
        /*
        if(pausePlayer.GetButton())
        {

        }
        */
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UnPauseGame()
    {
        pauseGo.SetActive(false);

        Time.timeScale = 1f;
    }
}
