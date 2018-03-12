using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaveType{
	IMPACT = 0,
	RECT_IMPACT = 1,
	ZONE = 2,
	ZONE_TEST = 3
}

public struct WaveOptions{
	public uint type;
	public Vector2 position;
	public Vector2 size;
	public uint state;
	public float stateTimeChange;
	public float amplitude;
	public float waveNumber;
	public float angularFrequency;
	public float waveSpeed;
	public float timeProgression;
	public float time;
	public float timeout;
	public Vector2 trash;

	public WaveOptions(WaveType type, Vector2 position, float amplitude, float waveLength, float period, float time, Vector2 size = new Vector2(), float waveSpeed= 0f, float timeProgression= 0f, float timeout= 0f){
		this.type = (uint) type;
		this.position = position;
		this.size = size;
		this.amplitude = amplitude;
		this.waveSpeed = waveSpeed;
		this.timeProgression = timeProgression;
		this.time = time;
		this.timeout = timeout;
		this.state = 0;
		this.stateTimeChange = 0f;
		this.trash = new Vector2();

		this.waveNumber = (2 * Mathf.PI) / waveLength;
		this.angularFrequency = (2 * Mathf.PI) / period;
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

	public static WaveOptions CreateImpact(Vector2 position, float amplitude, float waveLength, float period, float time, float waveSpeed, float timeDigress, float timeout){
		return new WaveOptions(WaveType.IMPACT, position, amplitude, waveLength, period, time, new Vector2(), waveSpeed, timeDigress, timeout);
	}

	public static WaveOptions CreateRectImpact(Vector2 position, Vector2 size, float amplitude, float waveLength, float period, float time, float waveSpeed, float timeDigress, float timeout){
		return new WaveOptions(WaveType.RECT_IMPACT, position, amplitude, waveLength, period, time, size, waveSpeed, timeDigress, timeout);
	}

	public static WaveOptions CreateZone(float amplitude, float waveLength, float period, float time){
		return new WaveOptions(WaveType.ZONE, new Vector2(), amplitude, waveLength, period, time);
	}

	public static WaveOptions CreateZoneTest(float amplitude, float waveLength, float period, float time){
		return new WaveOptions(WaveType.ZONE_TEST, new Vector2(), amplitude, waveLength, period, time);
	}

	public static bool IsTimeout(WaveOptions wave, float time){
		if((wave.type != ((uint) WaveType.ZONE)) && (wave.type != ((uint) WaveType.ZONE_TEST))){
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
