using UnityEngine;

/// <summary>
/// Setup all boats in the game depending the number of players and the customizations choosen.
/// </summary>
public class ShipManager : MonoBehaviour {

    public bool useDefaultConfig = false;
    public bool tweaking = false;

    public GameObject[] players;
    public ShipModulesManager[] shipModuleMgrs;
    
    private int playerAlive = 0;

    [Header("Modules for boat (Scriptable objects)")]
    public HarpoonModule[] harpoonsScriptObjs;
    public CoqueModule[] coquesScriptObjs;    
    public CabineModule[] cabinesScriptObjs;
    public BombStockModule[] bombsScriptObjs;

    private void Awake()
    {
        if(useDefaultConfig)
        {
            SetupAllShips();
        }
    }

    public void SetupAllShips()
    {
        playerAlive = GameManager.instance.nbOfPlayers;

        for (int i = 0; i < shipModuleMgrs.Length; i++)
        {
            // Check if a player is in the game.
            if(GameManager.instance.players[i])
            {
                // For debug only. Load default config.
                if(useDefaultConfig)
                {
                    shipModuleMgrs[i].ActivateShipModules(GameManager.instance.defaultConfig, this);
                }

                else
                {
                    shipModuleMgrs[i].ActivateShipModules(GameManager.instance.shipConfigs[i], this);
                }
                
                players[i].SetActive(true);
            }

            else
            {
                players[i].SetActive(false);
            }
        }
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
}
