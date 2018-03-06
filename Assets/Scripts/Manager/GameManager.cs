using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public bool debug;
	public float actualPlayer;

	public static GameManager instance {get; private set;}

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
