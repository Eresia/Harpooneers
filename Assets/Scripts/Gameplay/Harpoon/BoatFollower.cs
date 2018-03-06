using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatFollower : MonoBehaviour {

	[SerializeField]
	private Transform boat;

	private Transform selfTransform;
	
	private void Awake() {
		selfTransform = GetComponent<Transform>();
	}

	private void Update () {
		selfTransform.position = boat.position;
	}
}
