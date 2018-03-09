using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveImpact : Wave {

	public WaveImpact(WaveOptions options) : base(options){}

	public override float CalculateTheta(Vector2 offset, Vector2Int lod, float wt, Vector2 direction, Vector2 posAbs){
		float thetaX = (options.waveNumber * (offset.x / lod.x)) - (wt * direction.x);
		float thetaY = (options.waveNumber * (offset.y / lod.y)) - (wt * direction.y);

		float timeChange = 1 / Mathf.Exp(wt * options.timeDigress);

		float xHeight = Mathf.Sin(thetaX) / Mathf.Exp(posAbs.x * options.distanceDigress) * timeChange;
		float yHeight = Mathf.Sin(thetaY) / Mathf.Exp(posAbs.y * options.distanceDigress) * timeChange;
		
		float theta = (posAbs.x * xHeight + posAbs.y * yHeight) / (posAbs.x + posAbs.y);

		return theta;
	}

	protected override bool IsTooFar(Vector2 offset){
		return ((offset.x > 20) || (offset.y > 20));
	}
}
