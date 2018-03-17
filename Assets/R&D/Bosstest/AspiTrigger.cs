using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspiTrigger : MonoBehaviour {

	public AspiTentacleBehaviour tentacle;

	private void OnTriggerEnter(Collider other) {
		tentacle.BeginAttack();
	}
}
