using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomChooser : StateChooser {

	public int nbState;

	protected override int ChooseState(){
		return Random.Range(0, nbState);
	}
}
