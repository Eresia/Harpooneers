using Rewired;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

    public GameObject pauseGo;

    private Player player; // Rewired player.

   
    public Button menuButton;
    public Button quitButton;

    public Image[] buttonsImages;

    private bool _isPause;
    
    private void Awake() {
		GameManager.instance.pauseScript = this;
	}
    
    void Start()
    {
        pauseGo.SetActive(false);
    }

    public void PauseGame(int playerID)
    {
        pauseGo.SetActive(true);
        _isPause = true;
        player = ReInput.players.GetPlayer(playerID);

        Time.timeScale = 0f;
    }

    private void Update()
    {
        if(_isPause)
        {
            if (player.GetAxis("Move Vertical") > 0)
            {
                menuButton.gameObject.SetActive(true);
            }
            if (player.GetAxis("Move Vertical") < 0)
            {
                quitButton.gameObject.SetActive(true);
                quitButton.gameObject.SetActive(false);
            }
        }
        

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void UnPauseGame()
    {
        pauseGo.SetActive(false);
        _isPause = false;
        Time.timeScale = 1f;
    }
}
