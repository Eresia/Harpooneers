using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Setup all boats in the game depending the number of players and the customizations choosen.
/// </summary>
public class ShipManager : MonoBehaviour {

    public Material[] playerMaterials;
   
    public bool useDefaultConfig = false;
    public bool randomDefaultConfig = false;

    public ShipConfiguration defaultConfig = new ShipConfiguration
    {
        cabinId = 0,
        bombStockId = 0,
        coqueId = 0,
        harpoonId = 0
    };

    [Tooltip("Try to update module at runtime, but warning an attribute can not work.")]
    public bool tweaking = false;

    public GameObject[] players;
    
    private ShipModulesManager[] shipModuleMgrs;
    private PlayerManager[] playerMgrs;
    
    private int playerAlive = 0;

    [Header("Modules for boat (Scriptable objects)")]
    public HarpoonModule[] harpoonsScriptObjs;
    public CoqueModule[] coquesScriptObjs;    
    public CabineModule[] cabinesScriptObjs;
    public BombStockModule[] bombsScriptObjs;

    private void Awake()
    {
        shipModuleMgrs = new ShipModulesManager[players.Length];
        playerMgrs = new PlayerManager[players.Length];
        if (players.Length > 0)
        {
            for (int i = 0; i < players.Length; i++)
            {
                shipModuleMgrs[i] = players[i].GetComponentInChildren<ShipModulesManager>();
                playerMgrs[i] = shipModuleMgrs[i].GetComponent<PlayerManager>();
            }
        }

        if (useDefaultConfig)
        {
            SetupAllShips();
        }
    }

    public void SetupAllShips()
    {
        playerAlive = GameManager.instance.nbOfPlayers;

        for (int i = 0; i < players.Length; i++)
        {
            // Check if a player is in the game.
            if(GameManager.instance.players[i])
            {
                // For debug only. Load default config.
                if(useDefaultConfig)
                {
                    if (randomDefaultConfig)
                        RandomConfig();

                    shipModuleMgrs[i].ActivateShipModules(GameManager.instance.shipMgr.defaultConfig, this, playerMaterials[i]);
                }

                else
                {
                    shipModuleMgrs[i].ActivateShipModules(GameManager.instance.shipConfigs[i], this, playerMaterials[i]);
                }
                
                players[i].SetActive(true);
            }

            else
            {
                players[i].SetActive(false);
            }
        }
    }

    public void RandomConfig()
    {
        defaultConfig.bombStockId = Random.Range(0, 3);
        defaultConfig.harpoonId = Random.Range(0, 4);
        defaultConfig.cabinId = Random.Range(0, 4);
        defaultConfig.coqueId = Random.Range(0, 4);
    }

    public void NotifyDeath()
    {
        playerAlive--;

        if(playerAlive <= 0)
        {
            GameManager.instance.GameOver();
        }
    }

    public void NotifyAlive()
    {
        playerAlive++;
    }

    /// <summary>
    /// Return a random player alive to attack.
    /// </summary>
    /// <returns>Transform</returns>
    public Transform ChoosePlayerToAttack()
    {
        List<int> playerIds = new List<int>();
        for (int i = 0; i < GameManager.instance.nbOfPlayers; i++)
        {
            playerIds.Add(i);
        }

        bool targetFound = false;

        int playerId = 0;

        while (playerIds.Count > 0 && !targetFound)
        {
            playerId = playerIds[Random.Range(0, playerIds.Count)];

            if (!playerMgrs[playerId].IsDead)
            {
                targetFound = true;
            }

            else
            {
                playerIds.Remove(playerId);
            }
        }

        return players[playerId].transform.GetChild(0);
    }
}
