using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Sea : MonoBehaviour {

	public int size;
	public float colliderRadius;
	public float colliderSpace;
	public int layer;

	// [HideInInspector]
	// [HideInInspector]
	public Transform[] wave;

	private Transform selfTransform;

	private float time;

	private void Awake() {
		time = 0;
		selfTransform = GetComponent<Transform>();
	}

	private void Update() {
		if((GameManager.instance.actualPlayer) == -1 && (Input.GetMouseButtonDown(0))){

		}
		time += Time.deltaTime;
		for(int i = 0; i < size; i++){
			for(int j = 0; j < size; j++){
				Vector3 pos = wave[i*size + j].position;
				pos.y = Mathf.Sin(time * ((float) i) / 20f) / 3f;
				wave[i*size + j].position = pos;
			}
		}
	}

	public void CreateVortex(Vector2 position){
		Vector2 seaPosition = new Vector2(selfTransform.position.x - position.x, selfTransform.position.z - position.y);
	}
}
