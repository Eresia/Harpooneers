using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum WaveType{
	IMPACT = 0,
	RECT_IMPACT = 1,
	ZONE = 2,
	ZONE_TEST = 3,
	VORTEX = 4
}

public struct WaveOptions{
	public uint type;
	public Vector2 position;
	public Vector2 size;
	public uint state;
	public float stateTimeChange;
	public float amplitude;
	public float radius;
	public float smooth;
	public float waveNumber;
	public float angularFrequency;
	public float waveSpeed;
	public float timeProgression;
	public float time;
	public float timeout;

	public WaveOptions(WaveType type, Vector2 position, float amplitude, float radius, float smooth, float waveLength, float period, float time, Vector2 size = new Vector2(), float waveSpeed= 0f, float timeProgression= 0f, float timeout= 0f){
		this.type = (uint) type;
		this.position = position;
		this.size = size;
		this.amplitude = amplitude;
		this.radius = radius;
		this.smooth = smooth;
		this.waveSpeed = waveSpeed;
		this.timeProgression = timeProgression;
		this.time = time;
		this.timeout = timeout;
		this.state = 0;
		this.stateTimeChange = 0f;

		this.waveNumber = (2 * Mathf.PI) / waveLength;
		this.angularFrequency = (2 * Mathf.PI) / period;
	}
}

struct FrameOptions{
	public float time;
	public uint nbWaves;
	public float maxWaveHeight;
	public uint lod;
	public float ratio;
	public uint heigtMapRatio;
	public float minVariation;
	public float trash;
};

public class WaveManager{

	public float ActualTime {get; private set;}

	public Dictionary<int, WaveOptions> Waves {get; private set;}

	private int actualId;

	public WaveManager(){
		ResetTime();
		actualId = 0;
		Waves = new Dictionary<int, WaveOptions>();
	}

	public int CreateImpact(Vector2 position, float amplitude, float radius, float waveLength, float period, float waveSpeed, float timeDigress, float timeout){
		WaveOptions newWave = new WaveOptions(WaveType.IMPACT, position, amplitude, radius, 0f, waveLength, period, ActualTime, new Vector2(), waveSpeed, timeDigress, timeout);
		Waves.Add(actualId, newWave);
		actualId++;
		return actualId -1;
	}

	public int CreateRectImpact(Vector2 position, Vector2 size, float amplitude, float waveLength, float period, float waveSpeed, float timeDigress, float timeout){
		WaveOptions newWave = new WaveOptions(WaveType.RECT_IMPACT, position, amplitude, 0f, 0f, waveLength, period, ActualTime, size, waveSpeed, timeDigress, timeout);
		Waves.Add(actualId, newWave);
		actualId++;
		return actualId -1;
	}

	public int CreateZone(float amplitude, float waveLength, float period){
		WaveOptions newWave = new WaveOptions(WaveType.ZONE, new Vector2(), amplitude, 0f, 0f, waveLength, period, ActualTime);
		Waves.Add(actualId, newWave);
		actualId++;
		return actualId -1;
	}

	public int CreateZoneTest(float amplitude, float waveLength, float period){
		WaveOptions newWave = new WaveOptions(WaveType.ZONE_TEST, new Vector2(), amplitude, 0f, 0f, waveLength, period, ActualTime + 1);
		Waves.Add(actualId, newWave);
		actualId++;
		return actualId -1;
	}

	public int CreateVortex(Vector2 position, float amplitude, float radius, float smooth, float waveLength, float period, float timeDigress, float timeout){
		WaveOptions newWave = new WaveOptions(WaveType.VORTEX, position, amplitude, radius, smooth, waveLength, period, ActualTime, new Vector2(), 0f, timeDigress, timeout);
		Waves.Add(actualId, newWave);
		actualId++;
		return actualId -1;
	}

	public void ChangeWave(int id, WaveOptions wave){
		if(Waves.ContainsKey(id)){
			Waves[id] = wave;
		}
	}

	public void IncrementWaveState(int waveId){
		if(Waves.ContainsKey(waveId)){
			WaveOptions wave = Waves[waveId];
			wave.state += 1;
			wave.time = ActualTime;
			Waves[waveId] = wave;
		}
	}

	public bool IsTimeout(WaveOptions wave){
		if((wave.type == ((uint) WaveType.ZONE)) || (wave.type == ((uint) WaveType.ZONE_TEST))){
			return false;
		}
		else if(wave.type == ((uint) WaveType.VORTEX)){
			return ((wave.state == 1) && ((ActualTime - wave.stateTimeChange) > wave.timeout));
		}
		else{
			return ((ActualTime - wave.time) > wave.timeout);
		}
	}

	public void RefreshWaves(){
		int[] waveIds = Waves.Keys.ToArray();
		for(int i = 0; i < waveIds.Length; i++){
			if(IsTimeout(Waves[waveIds[i]])){
				Debug.Log("Remove Wave");
				Waves.Remove(waveIds[i]);
			}
		}
	}

	public void ResetTime(){
		ActualTime = 0f;
	}

	public void IncrementTime(float deltaTime){
		ActualTime += deltaTime;
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
