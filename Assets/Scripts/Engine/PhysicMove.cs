using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicMove : MonoBehaviour {

	public PhysicObject physicObject;

	public float friction;

	public float gravity;

	public Transform SelfTransform {get; private set;}

	public Vector3 Velocity {get; private set;}

    private void Awake() {
		SelfTransform = GetComponent<Transform>();
		Velocity = Vector3.zero;
	}

	private void Update() {

		SelfTransform.position += Velocity * Time.deltaTime;

        float inertness = 1 + (friction * Time.deltaTime);

        Velocity /= inertness;

        if (Velocity.sqrMagnitude < 0.01f){
			Velocity = Vector3.zero;
		}

        physicObject.MoveOnBoard(this);
    }

	public void AddForce(Vector3 force)
    {
		Velocity += force;
    }
}
