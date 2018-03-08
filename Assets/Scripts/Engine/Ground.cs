using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ground : MonoBehaviour {

	public struct TransformInfo{
		public Vector3 position;
		public Vector3 rotation;
	}
	
	private struct HeightInfo{
		public Vector2Int i;
		public Vector2Int j;
		public Vector4 coeff;

		public float diffX;

		public float diffY;
	}

	private struct WaveOptions{
		public Vector2 position;

		public float amplitude;

		public float waveLength;

		public float period;

		public float waveNumber;

		public float angularFrequency;

		public float time;
	}

	public Vector2Int lod;

	public float ratio;

	public float[] points;

	public float[] coeffPoints;

	private Transform selfTransform;

	private Vector2 halfLod;

	private float time;

	private List<WaveOptions> waves;

	[Space]

	public Color gizmosColor;

	public LayerMask testLayer;
	
	public float amplitude;

	public float waveLength;

	public float period;

	[Space]

	public float distanceDigress;

	public float timeDigress;

	public float minWave;

	private void Awake() {
		selfTransform = GetComponent<Transform>();
		halfLod = new Vector2(((float) lod.x) * 0.5f, ((float) lod.y) * 0.5f);
		waves = new List<WaveOptions>();
	}

	private void Update() {
		time += Time.deltaTime;
		if((GameManager.instance.actualPlayer == -1) && Input.GetMouseButtonDown(0)){
			RaycastHit hit;
        	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			// Debug.DrawRay()

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, testLayer)) {
				CreateVortex(hit.point);
			}
		}

		if(waves.Count > 0){
			WaveOptions[] currentWaves = waves.ToArray();

			for(int i = 0; i < lod.x; i++){
				for(int j = 0; j < lod.y; j++){
					if(coeffPoints[i * lod.y + j] != 0){
						points[i * lod.y + j] = 0;
						coeffPoints[i * lod.y + j] = 0;
					}
					
				}
			}

			for(int i = 0; i < currentWaves.Length; i++){
				CheckWave(currentWaves[i]);
			}

			for(int i = 0; i < lod.x; i++){
				for(int j = 0; j < lod.y; j++){
					if(coeffPoints[i * lod.y + j] != 0){
						points[i * lod.y + j] /= coeffPoints[i * lod.y + j];
					}
					
				}
			}
		}

	

		

		// for(int i = 0; i < lod.x; i++){
		// 	for(int j = 0; j < lod.y; j++){
		// 		points[i * lod.y + j] = Mathf.Sin(time * ((float) i) / 20f) / 3f;
		// 	}
		// }
	}

	private void CheckWave(WaveOptions w){
		double waveSize = 0f;
		for(int i = 0; i < 20; i++){
			for(int j = 0; j < 20; j++){
				waveSize += CalculateWave(w, i, j);
				waveSize += CalculateWave(w, -i, j);
				waveSize += CalculateWave(w, i, -j);
				waveSize += CalculateWave(w, -i, -j);

				// if(!result){
				// 	break;
				// }
				// Debug.Log(again);
				
			}

			// if(!result){
			// 	break;
			// }
		}

		if(waveSize < minWave){
			Debug.Log("Remove impact " + waveSize);
			waves.Remove(w);
		}
	}

	private float CalculateWave(WaveOptions w, int i, int j){

		if((i + j) == 0){
			return 0f;
		}

		float xRound = Mathf.Round(w.position.x);
		float yRound = Mathf.Round(w.position.y);

		if((xRound + i) < 0){
			return 0f;
		}

		if((xRound + i) >= lod.x){
			return 0f;
		}

		if((yRound + j) < 0){
			return 0f;
		}

		if((yRound + j) >= lod.y){
			return 0f;
		}

		int pointId = ((((int) xRound) + i) * lod.y) + (((int) yRound) + j);

		float offsetX = xRound - w.position.x;
		float offsetY = yRound - w.position.y;

		float currentTime = time - w.time;

		float iCoeff = 1;
		float jCoeff = 1;

		if(i < 0){
			iCoeff *= -1;
		}

		if(j < 0){
			jCoeff *= -1;
		}

		float wt = w.angularFrequency * currentTime;

		float iFloat = (float) i;
		float jFloat = (float) j;

		float iAbs = Mathf.Abs(iFloat);
		float jAbs = Mathf.Abs(jFloat);

		float thetaX = (w.waveNumber * (((float) i) / lod.x) - offsetX) - (wt * iCoeff);
		float thetaY = (w.waveNumber * (((float) j) / lod.y) - offsetY) - (wt * jCoeff);

		float xHeight;
		float yHeight;

		xHeight = Mathf.Sin(thetaX) / Mathf.Exp(iAbs / distanceDigress) / Mathf.Exp(wt / timeDigress);
		yHeight = Mathf.Sin(thetaY) / Mathf.Exp(jAbs / distanceDigress) / Mathf.Exp(wt / timeDigress);

		float coeff = iAbs + jAbs;
		float theta = (iAbs * xHeight + jAbs * yHeight) / coeff;

		float finalHeight = w.amplitude * theta;

		points[pointId] += finalHeight / coeff;
		coeffPoints[pointId] += (1 / coeff);

		// if(Mathf.Abs(finalHeight) < minWave){
		// 	return false;
		// }
		// else{
		// 	return true;
		// }

		return Mathf.Abs(finalHeight);
		
	}

	public void CreateVortex(Vector3 p){
		WaveOptions newWave = new WaveOptions();
		float iFloat = ((p.x / ratio) + halfLod.x) - selfTransform.position.x;
		float jFloat = ((p.z / ratio) + halfLod.y) - selfTransform.position.z;

		newWave.position = new Vector2(iFloat, jFloat);
		newWave.amplitude = amplitude;
		newWave.waveLength = waveLength;
		newWave.period = period;
		newWave.waveNumber = (2 * Mathf.PI) / waveLength;
		newWave.angularFrequency = (2 * Mathf.PI) / period;
		newWave.time = time;
		waves.Add(newWave);
	}

	private void OnDrawGizmos() {
		Transform seaTransform = GetComponent<Transform>();
		Vector2 halfLod = new Vector2(((float) lod.x) * 0.5f, ((float) lod.y) * 0.5f);
		Gizmos.color = gizmosColor;
		if(points.Length == (lod.x * lod.y)){
			for(int i = 0; i < lod.x; i++){
				for(int j = 0; j < lod.y; j++){
					if(i < (lod.x - 1)){
						Gizmos.DrawLine(CalculateRealPosition(i, j, halfLod, seaTransform), CalculateRealPosition(i + 1, j, halfLod, seaTransform));
					}

					if(j < (lod.y - 1)){
						Gizmos.DrawLine(CalculateRealPosition(i, j, halfLod, seaTransform), CalculateRealPosition(i, j + 1, halfLod, seaTransform));
					}
				}
			}
		}
	}

	public TransformInfo GetTransformInfo(Vector2 position, float yAngle){
		float minX = GetX(0);
		float maxX = GetX(lod.x - 1);
		float minZ = GetZ(0);
		float maxZ = GetZ(lod.y - 1);

		TransformInfo result = new TransformInfo();

		if(position.x < minX){
			result.position.x = minX;
		}
		else if(position.x > maxX){
			result.position.x = maxX;
		}
		else{
			result.position.x = position.x;
		}

		if(position.y < minZ){
			result.position.z = minZ;
		}
		else if(position.y > maxZ){
			result.position.z = maxZ;
		}
		else{
			result.position.z = position.y;
		}

		HeightInfo info = GetHeightInfo(result.position.x, result.position.z);
		result.position.y = GetHeight(info);
		result.rotation = GetRotation(info, result.position.y, yAngle);

		return result;
	}

	private Vector3 CalculateRealPosition(int i, int j){
		return CalculateRealPosition(i, j, halfLod, selfTransform);
	}

	private Vector3 CalculateRealPosition(int i, int j, Vector2 halfLod, Transform seaTransform){
		float x = GetX(i, halfLod, seaTransform);
		float y = points[i * lod.y + j];
		float z = GetZ(j, halfLod, seaTransform);
		return new Vector3(x, y, z);
	}

	private float GetHeight(HeightInfo info){
		float result;

		result = info.coeff.x * points[info.i.y * lod.y + info.j.y];
		result += info.coeff.y * points[info.i.x * lod.y + info.j.y];
		result += info.coeff.z * points[info.i.y * lod.y + info.j.x];
		result += info.coeff.w * points[info.i.x * lod.y + info.j.x];

		return result / (info.coeff.x + info.coeff.y + info.coeff.z + info.coeff.w);
	}

	private Vector3 GetRotation(HeightInfo info, float height, float yAngle){
		Vector3 result;

		// Vector3 a = new Vector3(0, points[info.i.x * lod.y + info.j.x], 0);
		// Vector3 b = new Vector3(1, points[info.i.y * lod.y + info.j.x], 0);
		// Vector3 c = new Vector3(0, points[info.i.x * lod.y + info.j.y], 1);
		// Vector3 d = new Vector3(1, points[info.i.y * lod.y + info.j.y], 1);

		// Vector3 cross1 = Vector3.Cross(b - a, c - a);
		// if(cross1.y < 0){
		// 	cross1 = -cross1;
		// }

		// Vector3 cross2 = Vector3.Cross(a - b, d - b);
		// if(cross2.y < 0){
		// 	cross2 = -cross2;
		// }

		// Vector3 cross3 = Vector3.Cross(a - c, d - c);
		// if(cross3.y < 0){
		// 	cross3 = -cross3;
		// }
		
		// Vector3 cross4 = Vector3.Cross(b - d, c - d);
		// if(cross4.y < 0){
		// 	cross4 = -cross4;
		// }

		// result = info.coeff.x * cross1; 
		// result += info.coeff.y * cross2; 
		// result += info.coeff.z * cross3; 
		// result += info.coeff.w * cross4;

		// return result / (info.coeff.x + info.coeff.y + info.coeff.z + info.coeff.w);

		float a = points[info.i.x * lod.y + info.j.x];
		float b = points[info.i.y * lod.y + info.j.x];
		float c = points[info.i.x * lod.y + info.j.y];
		float d = points[info.i.y * lod.y + info.j.y];

		if(yAngle < 0){
			yAngle = 360 - yAngle;
		}

		while(yAngle > 90){
			float temp = a;
			a = c;
			c = d;
			d = b;
			b = temp;
			yAngle -= 90;
		}

		

		float ab = Mathf.Abs(a-b);
		float cd = Mathf.Abs(c-d);
		float ac = Mathf.Abs(a-c);
		float bd = Mathf.Abs(b-d);

		float diffJ;
		float diffI;

		float signI;
		float signJ;

		if(ab > cd){
			diffJ = ab;
			signJ = Mathf.Sign(b-a);
		}
		else{
			diffJ = cd;
			signJ = Mathf.Sign(d-c);
		}

		if(ac > bd){
			diffI = ac;
			signI = Mathf.Sign(c-a);
		}
		else{
			diffI = bd; 
			signI = Mathf.Sign(d-b);
		}

		float x = - signI * Vector2.Angle(new Vector2(1, 0), new Vector2(1, diffI));
		float z = signJ * Vector2.Angle(new Vector2(1, 0), new Vector2(1, diffJ));

		// float minI = Mathf.Min(Mathf.Min(a, c), Mathf.Min(b, d));
		// float maxI = Mathf.Max(Mathf.Max(a, c), Mathf.Max(b, d));

		// float minJ = Mathf.Min(Mathf.Min(a, b), Mathf.Min(c, d));
		// float maxJ = Mathf.Max(Mathf.Max(a, b), Mathf.Max(c, d));

		// float x = Vector2.Angle(new Vector2(1, 0), new Vector2(1, maxI - minI));
		// float z = Vector2.Angle(new Vector2(1, 0), new Vector2(1, maxJ - minJ));
		result = new Vector3(x, 0, z);

		return result;
	}

	private HeightInfo GetHeightInfo(float x, float z){
		HeightInfo result = new HeightInfo();
		float iFloat = ((x / ratio) + halfLod.x) - selfTransform.position.x;
		float jFloat = ((z / ratio) + halfLod.y) - selfTransform.position.z;

		result.i.x = ((int) iFloat);
		result.i.y = result.i.x;

		result.j.x = ((int) jFloat);
		result.j.y = result.j.x;
		
		if(iFloat != result.i.x){
			result.i.y = result.i.x + 1;
		}

		if(jFloat != result.j.x){
			result.j.y = result.j.x + 1;
		}

		result.diffX = iFloat - ((float) result.i.x);
		result.diffY = jFloat - ((float) result.j.x);

		result.coeff.x = new Vector2(result.diffX, result.diffY).sqrMagnitude;
		result.coeff.y = new Vector2(1-result.diffX, result.diffY).sqrMagnitude;
		result.coeff.z = new Vector2(result.diffX, 1-result.diffY).sqrMagnitude;
		result.coeff.w = new Vector2(1-result.diffX, 1-result.diffY).sqrMagnitude;

		return result;
	}

	private float GetX(int i){
		return GetX(i, halfLod, selfTransform);
	}

	private float GetX(int i, Vector2 halfLod, Transform seaTransform){
		return seaTransform.position.x + ((((float) i) - halfLod.x) * ratio);
	}

	private float GetZ(int j){
		return GetZ(j, halfLod, selfTransform);
	}

	private float GetZ(int j, Vector2 halfLod, Transform seaTransform){
		return seaTransform.position.z + ((((float) j) - halfLod.y) * ratio);
	}
}
