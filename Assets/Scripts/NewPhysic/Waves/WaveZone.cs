using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveZone : Wave {

	public WaveZone(WaveOptions options) : base(options){}

	public override float CalculateTheta(Vector2 offset, Vector2Int lod, float wt, Vector2 direction, Vector2 posAbs){
		return Mathf.Sin((options.waveNumber * (offset.x / lod.x)) - wt);
	}

	protected override bool IsTooFar(Vector2 offset){
		return false;
	}

	public override bool IsTimeout(float time){
		return false;
	}
}
