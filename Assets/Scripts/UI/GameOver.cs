using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{

    public GameObject gameOverGo;

    public bool isGameOver;

	private void Awake() {
		GameManager.instance.gameOverScript = this;
	}

    // Use this for initialization
    void Start()
    {
        gameOverGo.SetActive(false);
    }

    public void DisplayGameOver()
    {
        isGameOver = true;
        gameOverGo.SetActive(true);
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }
}
