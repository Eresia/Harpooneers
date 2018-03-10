﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicObject : MonoBehaviour {

	[SerializeField]
	private Vector3 pivot;

	public Transform selfTransform {get; private set;}

	private void Awake() {
		selfTransform = GetComponent<Transform>();
	}

	public void MoveOnBoard(PhysicMove mover) {
		Vector2 pos2 = new Vector2(mover.SelfTransform.position.x, mover.SelfTransform.position.z);
		Ground.TransformInfo info = GameManager.instance.ground.GetTransformInfo(pos2, mover.SelfTransform.rotation.eulerAngles.y);

        selfTransform.localRotation = Quaternion.Euler(info.rotation);
		mover.SelfTransform.position = info.position;
		selfTransform.localPosition = -pivot;

		Vector3 normal = selfTransform.up;
		Vector3 gravity = new Vector3(0, -1, 0);

        Vector3 gravityForce = (normal + gravity) * mover.gravity * Time.deltaTime;

        mover.AddForce(gravityForce);
	}

	private void OnDrawGizmos() {
		Transform objectTransform = GetComponent<Transform>();
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(objectTransform.TransformPoint(pivot), 0.1f);
	}
}