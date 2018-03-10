using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct WaveOptions{
	public uint type;
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

	public WaveOptions(uint type, Vector2 position, float amplitude, float waveLength, float period, float time, float distanceDigress = 0f, float timeDigress= 0f, float timeout= 0f){
		this.type = type;
		this.position = position;
		this.amplitude = amplitude;
		this.waveLength = waveLength;
		this.period = period;
		this.distanceDigress = distanceDigress;
		this.timeDigress = timeDigress;
		this.time = time;
		this.timeout = timeout;

		this.waveNumber = (2 * Mathf.PI) / this.waveLength;
		this.angularFrequency = (2 * Mathf.PI) / this.period;
	}
}

struct FrameOptions{
	public float time;
	public uint nbWaves;
	public float maxWaveHeight;
	public uint lod;
	public uint heigtMapRatio;
	public Vector3 trash;
};

public class Wave{

	public static WaveOptions CreateImpact(Vector2 position, float amplitude, float waveLength, float period, float time, float distanceDigress, float timeDigress, float timeout){
		return new WaveOptions(0, position, amplitude, waveLength, period, time, distanceDigress, timeDigress, timeout);
	}

	public static WaveOptions CreateZone(Vector2 position, float amplitude, float waveLength, float period, float time){
		return new WaveOptions(1, position, amplitude, waveLength, period, time);
	}

	public static bool IsTimeout(WaveOptions wave, float time){
		if(wave.type != 1){
			return (time - wave.time) > wave.timeout;
		}
		else{
			return false;
		}
	}
}

public abstract class Wave_a {

	protected WaveOptions options {get; private set;}

	public Wave_a(WaveOptions options){
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
