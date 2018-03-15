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

    void Update()
    {
       
    }

    public void PauseGame(int playerID)
    {
        _isGamePaused = true;
        pauseGo.SetActive(true);
        GameManager.instance.bossMgr.DisplayLifeBar(false);
        Time.timeScale = 0f;
        Debug.Log(playerID);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UnPauseGame()
    {
        _isGamePaused = false;
        pauseGo.SetActive(false);
        GameManager.instance.bossMgr.DisplayLifeBar(true);
        Time.timeScale = 1f;
    }
}
