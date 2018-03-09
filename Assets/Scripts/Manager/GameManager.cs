using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public bool debug;
	public int actualPlayer;

    public ScenesManager sceneMgr;
    public BoundaryManager boundaryMgr;
    public ShipManager shipMgr;

    public Ground ground;

	public static GameManager instance {get; private set;}

    /// <summary>
    /// Return the number of players for the current game.
    /// </summary>
    public int nbOfPlayers;

    public ShipConfiguration[] shipConfigs;

    public bool[] players;

    public ShipConfiguration defaultConfig = new ShipConfiguration
    {
        cabinId = 0,
        bombStockId = 0,
        coqueId = 0,
        harpoonId = 0
    };
    
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
    }

    /// <summary>
    /// Setup the game scene. Retrieve specific manager.
    /// </summary>
    public void SetupGameScene()
    {
        boundaryMgr = FindObjectOfType<BoundaryManager>();
        shipMgr = FindObjectOfType<ShipManager>();
        ground = FindObjectOfType<Ground>();

        shipMgr.SetupAllShips();
    }

    public void StartNewGame(int playerCount, bool[] playersReady)
    {
        nbOfPlayers = playerCount;
        players = playersReady;

        sceneMgr.LoadGameScene();
    }

    public void ReturnToMainMenu(bool goToCustomizationMenu)
    {
        // TODO store in a bool if we want to go directly in customization screen.

        sceneMgr.LoadMainMenuScene();
    }
    
    public void GameOver()
    {
        Debug.Log("GAME OVER !!!");
    }
}
