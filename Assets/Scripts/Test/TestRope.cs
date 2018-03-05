using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TestRope : MonoBehaviour {

	public Rope ropePrefab;

	public Transform ropeBegin;

	public Transform target;

	private Transform selfTransform;

	private void Awake()
	{
		selfTransform = GetComponent<Transform>();
		Grip(target);
	}

	public void Grip(Transform target){
		Rope newRope = Instantiate<Rope>(ropePrefab);
		newRope.selfTransform.parent = target;
		newRope.selfTransform.localPosition = Vector3.zero;
		newRope.SetRope(ropeBegin, target, Vector3.Distance(ropeBegin.position, target.position) + 0.5f);
		Rigidbody ropeRigid = GetComponentInChildren<UltimateRopeLink>().GetComponent<Rigidbody>();
		ropeRigid.isKinematic = false;
	}
}
