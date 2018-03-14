using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    public GameObject pauseGo;

    private bool _isGamePaused;

    private int _playerNb;

    void Start()
    {
        pauseGo.SetActive(false);
        _playerNb = GameManager.instance.nbOfPlayers;
    }

    public void PauseGame(int playerID)
    {
        _isGamePaused = true;
        pauseGo.SetActive(true);
        Time.timeScale = 0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UnPauseGame()
    {
        _isGamePaused = false;
        pauseGo.SetActive(false);
        Time.timeScale = 1f;
    }
}
