using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public bool debug;
	public int actualPlayer;

    public ScenesManager sceneMgr;
    public CameraManager camMgr;
	public AudioManager audioManager;

    // Only in game components.
    public Tutorial tutorial;

    public BossManager bossMgr;
    public ShipManager shipMgr;
    public BoundaryManager boundaryMgr;
    public Ground ground;

    public CinematicMgr cinematicMgr;
    public FadeInOut FadeMgr;
    public EndScreenMgr endScreenMgr;

	[Space]

	public bool tutoEnabled = true;

	public static GameManager instance {get; private set;}
    
    public bool IsEndScreen;
    private bool win;
    
    public Pause pauseScript;
    public GameOver gameOverScript;

    /// <summary>
    /// Return the number of players for the current game.
    /// </summary>
    [HideInInspector]
    public int nbOfPlayers;

    public ShipConfiguration[] shipConfigs;

    public bool[] players;
    
    void Awake()
	{
		if (instance == null) {
			instance = this;
		}

		else if (instance != this) {
			Destroy(gameObject);
            return;
		}
        
        DontDestroyOnLoad(gameObject);
        Application.runInBackground = true;

        SetupGame();
	}
    
    // Setup or reset data.
    private void SetupGame()
    {
        shipConfigs = new ShipConfiguration[4];

        // For debug all players are enabled.
        players = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            players[i] = true;
        }
        
        SetupMainMenu();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if(debug)
        {
            return;
        }

        DynamicGI.UpdateEnvironment();

        if (scene.buildIndex == 0)
        {
            SetupMainMenu();
        }

        else if (scene.buildIndex == 1)
        {
            SetupGameScene();
        }
    }
    
    /// <summary>
    /// Setup the main menu scene. Retrieve specific manager.
    /// </summary>
    public void SetupMainMenu()
    {
        if(!shipMgr)
        {
            shipMgr = FindObjectOfType<ShipManager>();
        }

        audioManager.PlayTutoMusic();
    }

    /// <summary>
    /// Setup the game scene. Retrieve specific manager.
    /// </summary>
    public void SetupGameScene()
    {
        boundaryMgr = FindObjectOfType<BoundaryManager>();
        shipMgr = FindObjectOfType<ShipManager>();
        ground = FindObjectOfType<Ground>();
        camMgr = FindObjectOfType<CameraManager>();
        bossMgr = FindObjectOfType<BossManager>();
        
		win = false;

        shipMgr.SetupAllShips();

        if(FadeMgr != null)
            FadeMgr.FadeIn();

        audioManager.PlayTutoMusic();
    }

    public void StartNewGame(int playerCount, bool[] playersReady)
    {
        nbOfPlayers = playerCount;
        players = playersReady;

        FadeMgr.FadeOut(sceneMgr.LoadGameScene);
    }

    public void ReturnToMainMenu()
    {
        sceneMgr.LoadMainMenuScene();
    }
    
    public void PauseGame(int playerID)
    {             
        pauseScript.PauseGame(playerID);

        if (bossMgr.hasSpawn)
        {
            bossMgr.DisplayLifeBar(false);
        }
    }

    public void UnPauseGame()
    {
        pauseScript.UnPauseGame();

        if (bossMgr.hasSpawn)
        {
            bossMgr.DisplayLifeBar(true);
        }
    }

    public void PressButton(int playerID)
    {
        pauseScript.ButtonPress(playerID);
    }
    
    public void GameOver()
    {
        if(gameOverScript.isGameOver)
        {
            return;
        }
    
        gameOverScript.DisplayGameOver();
    }
    
    [ContextMenu("FAKE END")]
    public void GameFinished()
    {
        if (win)
        {
            return;
        }

        win = true;
        IsEndScreen = false; // Wait the end screen to fade in.

        FadeMgr.FadeOut(DisplayEndScreen);
    }

    public void DisplayEndScreen()
    {
        endScreenMgr.Display();
    }

	public void OnEndTuto(){
		tutoEnabled = false;
		bossMgr.enabled = true;
		shipMgr.ResurrectAll();

        audioManager.PlayFightMusic();
	}
}
