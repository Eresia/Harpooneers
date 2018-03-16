using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AspiBomb : MonoBehaviour {

	public Action OnExplodeAction;

	public Collider[] Colliders {get; private set;}

	private void Awake() {
		Colliders = GetComponents<Collider>();
		foreach(Collider c in Colliders){
			c.enabled = false;
		}
	}

	public void OnExplode()
    {
        OnExplodeAction();
    }
}
