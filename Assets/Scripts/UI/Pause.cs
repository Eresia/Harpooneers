using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    public GameObject pauseGo;
    
    void Start()
    {
        pauseGo.SetActive(false);
    }

    public void PauseGame(int playerID)
    {
        pauseGo.SetActive(true);
        
        Time.timeScale = 0f;
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
