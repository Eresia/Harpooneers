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

	public void MoveOnBoard(PhysicMove mover) {
		Vector2 pos2 = new Vector2(mover.SelfTransform.position.x, mover.SelfTransform.position.z);
		Ground.TransformInfo info = GameManager.instance.ground.GetTransformInfo(pos2);
		selfTransform.up = info.normal;
		Vector3 eulerAngles = selfTransform.localEulerAngles;
		float y = mover.SelfTransform.rotation.eulerAngles.y;
		if(y > 180){
			y -= 360;
		}
		if(eulerAngles.x > 180){
			eulerAngles.x -= 360;
		}
		if(eulerAngles.z > 180){
			eulerAngles.z -= 360;
		}
		float x = (eulerAngles.x * (1 - (y / 90))) + (eulerAngles.z * (y / 90));
		float z = (eulerAngles.z * (1 - (y / 90))) + (eulerAngles.x * (y / 90));
		selfTransform.localEulerAngles = new Vector3(x, 0, z);
		mover.SelfTransform.position = info.position;
		selfTransform.localPosition = -pivot;
		Vector3 normal = info.normal;
		Vector3 gravity = new Vector3(0, -1, 0);
		mover.AddForce((normal + gravity) * mover.gravity * Time.deltaTime);
	}

	private void OnDrawGizmos() {
		Transform objectTransform = GetComponent<Transform>();
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(objectTransform.TransformPoint(pivot), 0.1f);
	}
}
