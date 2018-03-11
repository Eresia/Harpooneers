using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : BossState<DashPattern> {

	protected override DashPattern Init(){
		return new DashPattern();
	}
}
