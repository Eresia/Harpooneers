using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicObject : MonoBehaviour {

	[SerializeField]
	private Vector3 pivot;

	public Transform selfTransform {get; private set;}

	private void Awake() {
		selfTransform = GetComponent<Transform>();
	}

	public void MoveOnBoard(Transform mover) {
		Vector2 pos2 = new Vector2(mover.position.x, mover.position.z);
		Ground.TransformInfo info = GameManager.instance.ground.GetTransformInfo(pos2);
		selfTransform.localRotation = Quaternion.LookRotation(info.rotation);
		selfTransform.Rotate(Vector3.right * 90f);
		mover.position = info.position;
		selfTransform.localPosition = -pivot;

		Debug.DrawRay(selfTransform.position, info.rotation);
	}

	private void OnDrawGizmos() {
		Transform objectTransform = GetComponent<Transform>();
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(objectTransform.TransformPoint(pivot), 0.1f);
	}
}
