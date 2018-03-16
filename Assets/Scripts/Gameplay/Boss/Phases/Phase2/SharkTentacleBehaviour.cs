using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class SharkTentacleBehaviour : TentacleBehaviour {

	[Space]

	public AspiBomb aspiBomb;

	public float bombDamages;

    public void BeginAttack(){
		animator.SetTrigger("Eat");
	}
}
