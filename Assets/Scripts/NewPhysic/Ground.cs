using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	public Vector2Int lod;

	public float ratio;

	public float[] points;

	private Transform selfTransform;

	private Vector2 halfLod;

	private float time;

	private void Awake() {
		selfTransform = GetComponent<Transform>();
		halfLod = new Vector2(((float) lod.x) * 0.5f, ((float) lod.y) * 0.5f);
	}

	// private void Update() {
	// 	time += Time.deltaTime;
	// 	for(int i = 0; i < lod.x; i++){
	// 		for(int j = 0; j < lod.y; j++){
	// 			points[i * lod.y + j] = Mathf.Sin(time * ((float) i) / 20f) / 3f;
	// 		}
	// 	}
	// }

	private void OnDrawGizmos() {
		Transform seaTransform = GetComponent<Transform>();
		Vector2 halfLod = new Vector2(((float) lod.x) * 0.5f, ((float) lod.y) * 0.5f);
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

	public TransformInfo GetTransformInfo(Vector2 position){
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
		result.rotation = GetRotation(info, result.position.y);

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

	private Vector3 GetRotation(HeightInfo info, float height){
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
