using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicMove : MonoBehaviour {

	public PhysicObject physicObject;

	public float friction;

	public float mass;

	public Transform selfTransform {get; private set;}

	public Vector3 acceleration;

	public float deceleration;

	public Vector3 velocity {get; private set;}

	private void Awake() {
		selfTransform = GetComponent<Transform>();
		velocity = Vector3.zero;
	}

	private void Update() {
		velocity += acceleration;
		selfTransform.position += velocity;
		AddForce(-velocity.normalized * friction);
		if(velocity.sqrMagnitude < friction){
			velocity = Vector3.zero;
		}
		
		physicObject.MoveOnBoard(selfTransform);
	}

	private void AddForce(Vector3 force){
		acceleration += force / mass;
	}

}
