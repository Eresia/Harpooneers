using Rewired;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour {

    public GameObject pauseGo;

    private Player player; // Rewired player.

   
    public Button menuButton;
    public Button quitButton;

    public TextMeshProUGUI menuText;
    public TextMeshProUGUI quitText;

    public Sprite[] buttonsImages;

    private bool _isPause;
    private bool quitSelected;

    private int playerIdControl;
    
    private void Awake() {
		GameManager.instance.pauseScript = this;
	}
    
    void Start()
    {
        pauseGo.SetActive(false);
    }

    public void PauseGame(int playerID)
    {

        playerIdControl = playerID;
        pauseGo.SetActive(true);
        _isPause = true;
        player = ReInput.players.GetPlayer(playerID);

        Time.timeScale = 0f;

        quitSelected = false;
        ChangeButtonFocus();
    }

    private void Update()
    {
        if(_isPause)
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
        if(_isPause && playerID == playerIdControl)
        {
            if (quitSelected)
            {
               
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }  
    }

    public void UnPauseGame()
    {
        pauseGo.SetActive(false);
        _isPause = false;
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
