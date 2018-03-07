using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public bool debug;
	public int actualPlayer;

    public ScenesManager sceneMgr;

    public BoundaryManager boundaryMgr;

	public static GameManager instance {get; private set;}

    /// <summary>
    /// Return the number of players for the current game.
    /// </summary>
    public int nbOfPlayers;

    public ShipConfiguration[] shipConfigs;

    public ShipConfiguration defaultConfig = new ShipConfiguration
    {
        cabinId = 0,
        bombStockId = 0,
        coqueId = 0,
        harpoonId = 0
    };

    private ShipManager shipMgr;
    
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
        if(scene.buildIndex == 1)
        {
            SetupGameScene();
        }
    }

    /// <summary>
    /// Setup the game scene. Retrieve specific manager.
    /// </summary>
    public void SetupGameScene()
    {
        boundaryMgr = FindObjectOfType<BoundaryManager>();
        shipMgr = FindObjectOfType<ShipManager>();

        shipMgr.SetupAllShips();
    }

    public void StartNewGame(int playerCount)
    {
        nbOfPlayers = playerCount;

        sceneMgr.LoadGameScene();
    }

    public void ReturnToMainMenu()
    {
        sceneMgr.LoadMainMenuScene();
    }
}
