using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WaveOptions{
	public Vector2 position;

	public float amplitude;

	public float waveLength;

	public float period;

	public float waveNumber;

	public float angularFrequency;

	public float distanceDigress;

	public float timeDigress;

	public float time;

	public float timeout;
}

public abstract class Wave {

	protected WaveOptions options {get; private set;}

	public Wave(WaveOptions options){
		this.options = options;
	}

	public virtual Vector2 CalculateWave(Vector2Int pos, float time, Vector2Int lod){
		Vector2 offset = new Vector2(options.position.x - pos.x, options.position.y - pos.y);

		if((offset.sqrMagnitude == 0) || IsTooFar(offset)){
			return Vector2.zero;
		}

		Vector2 direction = new Vector2(Mathf.Sign(offset.x), Mathf.Sign(offset.y));

		Vector2 posAbs = new Vector2(Mathf.Abs(offset.x), Mathf.Abs(offset.y));

		float coeff = (posAbs.x + posAbs.y);
		
		float currentTime = time - options.time;
		float wt = options.angularFrequency * currentTime;

		float theta = CalculateTheta(offset, lod, wt, direction, posAbs);

		float finalHeight = options.amplitude * theta;

		float heightAbs = Mathf.Abs(finalHeight);

		return new Vector2(finalHeight * (heightAbs/ coeff), (heightAbs / coeff));
	}

	public abstract float CalculateTheta(Vector2 offset, Vector2Int lod, float wt, Vector2 direction, Vector2 posAbs);

	protected abstract bool IsTooFar(Vector2 offset);

	public virtual bool IsTimeout(float time){
		return (time - options.time) > options.timeout;
	}
}
