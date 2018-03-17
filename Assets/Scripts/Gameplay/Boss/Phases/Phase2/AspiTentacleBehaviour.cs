using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspiTentacleBehaviour : TentacleBehaviour {

	[Space]

	public AspiBomb[] aspiBombs;

	public float bombDamages;

    public void BeginAttack()
    {
		animator.SetTrigger("Eat");
	}
}
