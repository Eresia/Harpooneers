using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverGo;

    public bool isGameOver;

	private void Awake()
    {
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
}
