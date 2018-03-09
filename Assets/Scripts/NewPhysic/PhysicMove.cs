using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PhysicMove : MonoBehaviour {

	public PhysicObject physicObject;

	public float friction;

	public float mass;

	public float limitSpeed;

	public float gravity;

	public Transform selfTransform {get; private set;}

	public Vector3 velocity {get; private set;}

	private void Awake() {
		selfTransform = GetComponent<Transform>();
		velocity = Vector3.zero;
	}

	private void Update() {
		selfTransform.position += velocity * Time.deltaTime;
		velocity /= 1 + (friction * Time.deltaTime);
		if(velocity.sqrMagnitude < 0.01f){
			velocity = Vector3.zero;
		}

		physicObject.MoveOnBoard(this);

		velocity = Vector3.ClampMagnitude(velocity, limitSpeed);
	}

	public void AddForce(Vector3 force){
		velocity += force / mass;
	}

}
