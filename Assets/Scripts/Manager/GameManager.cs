using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public bool debug;
	public int actualPlayer;

    public BoundaryManager boundaries;

	public static GameManager instance {get; private set;}

    /// <summary>
    /// Return the number of players for the current game.
    /// </summary>
    public int nbOfPlayers;

    public BoatConfiguration[] shipConfigs;

    private ShipManager moduleMgr;
    
    void Awake()
	{
		if (instance == null){
			instance = this;
		}

		else if (instance != this){
			Destroy(gameObject);   
		}
	}
}
