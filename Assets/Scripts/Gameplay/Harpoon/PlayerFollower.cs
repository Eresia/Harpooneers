using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour {

	[SerializeField]
	private Transform player;

	private Transform selfTransform;
	
	private void Awake() {
		selfTransform = GetComponent<Transform>();
	}

    private void Reset()
    {
        player = transform.parent.GetChild(0);
    }

    private void Update () {
		selfTransform.position = player.position;
	}
}
