using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UltimateRope))]
public class Rope : MonoBehaviour {

	[SerializeField]
	private UltimateRope ultimateRope;

	public Transform selfTransform {get; private set;}

	// private Transform owner;
	// private Transform target;

	private void Awake() {
		ultimateRope = GetComponent<UltimateRope>();
		selfTransform = GetComponent<Transform>();
	}

	// private void Update()
	// {
	// 	if((owner == null) || (target == null)){
	// 		return ;
	// 	}

	// 	ultimateRope.RopeNodes[0].fLength = Vector3.Distance(owner.position, target.position);
	// }

	public void SetRope(Transform owner, Transform target, float length){
		// this.owner = owner;
		// this.target = target;
		ultimateRope.RopeStart = owner.gameObject;
		ultimateRope.RopeNodes[0].goNode = target.gameObject;
		ultimateRope.RopeNodes[0].fLength = length;
		ultimateRope.Regenerate(ultimateRope.HasDynamicSegmentNodes());
	}
}
