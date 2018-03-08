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
		Ground.TransformInfo info = GameManager.instance.ground.GetTransformInfo(pos2, mover.rotation.eulerAngles.y);
		Vector3 realRotation = info.rotation;
		// int offset = (int) ((mover.rotation.eulerAngles.y + 45) / 90);
		// Debug.Log(mover.rotation.eulerAngles.y);
		// realRotation.x -= realRotation.x * offset;
		// realRotation.z -= realRotation.z * offset;
		selfTransform.localRotation = Quaternion.Euler(realRotation);
		// selfTransform.Rotate(Vector3.right * 90f);
		mover.position = info.position;
		selfTransform.localPosition = -pivot;
	}

	private void OnDrawGizmos() {
		Transform objectTransform = GetComponent<Transform>();
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(objectTransform.TransformPoint(pivot), 0.1f);
	}
}
