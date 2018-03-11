using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DashPattern : BossPattern {

    protected override void ExecutePattern(){
		boss.StartCoroutine(TempDash());
	}

	private IEnumerator TempDash()
	{
		yield return new WaitForSeconds(2f);
		FinishPattern();
	}
}
