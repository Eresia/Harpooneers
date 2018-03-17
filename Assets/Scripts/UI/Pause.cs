using UnityEngine;
using UnityEngine.UI;

using Rewired;
using TMPro;

public class Pause : MonoBehaviour {

    public GameObject pauseGo;
    
    public Button menuButton;
    public Button quitButton;

    public TextMeshProUGUI menuText;
    public TextMeshProUGUI quitText;

    public Sprite[] buttonsImages;

    public bool IsPause { get; private set; }

    private bool quitSelected;

    public int playerIdControl;
    private Player player; // Rewired player.

    private void Awake()
    {
		GameManager.instance.pauseScript = this;
	}
    
    void Start()
    {
        pauseGo.SetActive(false);
    }

    public void PauseGame(int playerID)
    {
        playerIdControl = playerID;
        player = ReInput.players.GetPlayer(playerID);

        pauseGo.SetActive(true);
        IsPause = true;

        Time.timeScale = 0f;

        quitSelected = false;

        ChangeButtonFocus();
    }

    private void Update()
    {
        if(IsPause)
        {
            if (player.GetAxis("Move Vertical") < 0)
            {
                quitSelected = true;
            }

            if (player.GetAxis("Move Vertical") > 0)
            {
                quitSelected = false;
            }

            ChangeButtonFocus();
        }
    }

    public void ButtonPress(int playerID)
    {
        if(IsPause && playerID == playerIdControl)
        {
            if (quitSelected)
            {
                Application.Quit();
            }

            else
            {
                GameManager.instance.sceneMgr.LoadMainMenuScene();
            }
        }  
    }

    public void UnPauseGame()
    {
        IsPause = false;

        pauseGo.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ChangeButtonFocus()
    {
        if(quitSelected)
        {
            menuButton.image.sprite = buttonsImages[0];
            quitButton.image.sprite = buttonsImages[3];
            
            menuText.color = new Color(1, 1f, 1f);
            quitText.color = new Color(1, 0.75f, 0.5f);
        }

        else
        {
            menuButton.image.sprite = buttonsImages[1];
            quitButton.image.sprite = buttonsImages[2];

            menuText.color = new Color(1, 0.75f, 0.5f);
            quitText.color = new Color(1, 1f, 1f);
        }
    }
}
