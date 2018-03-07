using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Rewired;

public class Sea : MonoBehaviour {

	public int size;
	public float colliderRadius;
	public float colliderSpace;
	public int layer;

	// [HideInInspector]
	// [HideInInspector]
	public Transform[] wave;

	private Transform selfTransform;

	private Transform cameraTransform;

	private Mouse mouse;

	private float time;

	private void Awake() {
		// time = 0;
		selfTransform = GetComponent<Transform>();
		cameraTransform = Camera.main.GetComponent<Transform>();
		mouse = ReInput.controllers.Mouse;
	}

	private void Update() {
		if((GameManager.instance.actualPlayer == -1) && mouse.GetButtonDown(0)){
			RaycastHit hit;
        	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			// Debug.DrawRay()

			if (Physics.Raycast(ray, out hit, Vector3.Distance(cameraTransform.position, selfTransform.position), layer)) {
				CreateVortex(hit.transform.position);
			}
		}
		time += Time.deltaTime;
		for(int i = 0; i < size; i++){
			for(int j = 0; j < size; j++){
				Vector3 pos = wave[i*size + j].position;
				pos.y = Mathf.Sin(time * ((float) i) / 2f) / 3f;
				wave[i*size + j].position = pos;
			}
		}
	}

	public void CreateVortex(Vector3 position){
		Vector3 seaPosition = selfTransform.position - position;
		int i = (int) seaPosition.x - (size / 2);
		int j = (int) seaPosition.z - (size / 2);
		wave[i*size + j].position += new Vector3(0, 1, 0);
	}
}
