using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[RequireComponent(typeof(Renderer))]
public class Ground : MonoBehaviour {

	public struct TransformInfo{
		public Vector3 position;
		public Vector3 normal;
	}
	
	private struct HeightInfo{
		public Vector2Int i;
		public Vector2Int j;
		public Vector4 coeff;

		public float diffX;

		public float diffY;
	}

	public int lodPower;

	public int heigtMapPower;

	public float ratio;

	public float maxWaveHeight;

	public ComputeShader seaCompute;

	public float[] points;

	private Vector3[] normales;

	private Transform selfTransform;

	private int lod;

	private float halfLod;

	private float time;
	

	private RenderTexture heightMapTexture;

	private RenderTexture normaleMapTexture;

	private List<WaveOptions> waves;

	private ComputeBuffer optionBuffer;

	private ComputeBuffer pointBuffer;

	private ComputeBuffer normaleBuffer;

	private FrameOptions[] frameOptions;

	private int lodPowPower;

	[Space]

	public bool displayGizmos;
	public Color gizmosColor;

	public LayerMask testLayer;


	[Space]

	[Header("Zone Waves options")]

	public float zoneAmplitude;

	public float zoneWaveLength;

	public float zonePeriod;

	[Space]

	[Header("Impact Waves options")]

	public float impactAmplitude;

	public float impactWaveLength;

	public float impactPeriod;

	[Range(0f, 1f)]
	public float distanceDigress;

	[Range(0f, 1f)]
	public float timeDigress;

	public float timeout;

	public RawImage rawImage;

	private Material material;

	private void Awake() {
		selfTransform = GetComponent<Transform>();
		lodPowPower = ((int) Mathf.Pow(2, lodPower));
		lod = 32 * lodPowPower;
		normales = new Vector3[lod * lod];
		halfLod = ((float) lod) * 0.5f;
		waves = new List<WaveOptions>();
		if(zoneAmplitude != 0){
			CreateZone();
		}

		int heigtMapLod = 32 * ((int) Mathf.Pow(2, heigtMapPower));

		heightMapTexture = new RenderTexture(heigtMapLod, heigtMapLod, 24);
		heightMapTexture.name = "HeightMap";
		heightMapTexture.enableRandomWrite = true;
		heightMapTexture.Create();

		normaleMapTexture = new RenderTexture(heigtMapLod, heigtMapLod, 24);
		normaleMapTexture.name = "NormaleMap";
		normaleMapTexture.enableRandomWrite = true;
		normaleMapTexture.Create();

		frameOptions = new FrameOptions[1];
		frameOptions[0] = new FrameOptions();
		frameOptions[0].maxWaveHeight = maxWaveHeight;
		frameOptions[0].heigtMapRatio = (uint) (heigtMapLod / lod);

		pointBuffer = new ComputeBuffer(points.Length, sizeof(float));
		normaleBuffer = new ComputeBuffer(normales.Length, 3 * sizeof(float));

		optionBuffer = new ComputeBuffer(1, 8 * sizeof(float));

		int pointKernel = seaCompute.FindKernel("CalculatePoint");
		int normaleKernel = seaCompute.FindKernel("CalculateNormal");

		seaCompute.SetBuffer(pointKernel, "Options", optionBuffer);
		seaCompute.SetBuffer(pointKernel, "Result", pointBuffer);
		seaCompute.SetTexture(pointKernel, "HeightMap", heightMapTexture);

		seaCompute.SetBuffer(normaleKernel, "Options", optionBuffer);
		seaCompute.SetBuffer(normaleKernel, "Result", pointBuffer);
		seaCompute.SetBuffer(normaleKernel, "Normales", normaleBuffer);
		seaCompute.SetTexture(normaleKernel, "NormaleMap", normaleMapTexture);

		material = GetComponent<Renderer>().material;
		material.SetTexture("_HeightMap", normaleMapTexture);
		material.SetTexture("_NormaleMap", normaleMapTexture);
		material.SetBuffer("_Vertex", pointBuffer);
		material.SetFloat("_MaxWaveHeight", maxWaveHeight);
		material.SetInt("_VertexSize", lod);

		if(rawImage != null){
			rawImage.texture = heightMapTexture;
		}
	}

	private void Update() {
		time += Time.deltaTime;
		if((GameManager.instance.actualPlayer == -1) && Input.GetMouseButtonDown(0)){
			RaycastHit hit;
        	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			// Debug.DrawRay()

			if (Physics.Raycast(ray, out hit, Mathf.Infinity, testLayer)) {
				CreateImpact(hit.point);
			}
		}

		WaveOptions[] waveArray = waves.ToArray();
		
		if(waves.Count > 0){
			// for(int i = 0; i < lod; i++){
			// 	for(int j = 0; j < lod; j++){
			// 		CalculateWave(new Vector2Int(i, j));
			// 	}
			// }

			frameOptions[0].time = time;
			frameOptions[0].nbWaves = (uint) waveArray.Length;
			frameOptions[0].lod = (uint) lod;
			optionBuffer.SetData(frameOptions);

			ComputeBuffer impacts = new ComputeBuffer(waveArray.Length, 12 * sizeof(float));
			impacts.SetData(waves);

			pointBuffer.SetData(points);

			int pointKernel = seaCompute.FindKernel("CalculatePoint");
			int normaleKernel = seaCompute.FindKernel("CalculateNormal");

			seaCompute.SetBuffer(pointKernel, "Impacts", impacts);

			seaCompute.Dispatch(pointKernel, lodPowPower, lodPowPower, 1);
			seaCompute.Dispatch(normaleKernel, lodPowPower, lodPowPower, 1);

			pointBuffer.GetData(points);
			normaleBuffer.GetData(normales);

			impacts.Dispose();
		}

		for(int i = 0; i < waveArray.Length; i++){
			if(Wave.IsTimeout(waveArray[i], time)){
				Debug.Log("Remove Wave");
				waves.Remove(waveArray[i]);
			}
		}	

		// heightMapTexture.SetPixels(heigthMap);
		// heightMapTexture.Apply();

	
		// for(int i = 0; i < lod; i++){
		// 	for(int j = 0; j < lod; j++){
		// 		points[i * lod + j] = Mathf.Sin(time * ((float) i) / 20f) / 3f;
		// 	}
		// }
	}

	// private void CalculateWave(Vector2Int pos){

	// 	Vector2 heighInfo = Vector2.zero;
	// 	int pointId = (pos.x* lod) + pos.y;

	// 	foreach(Wave_a w in waves){
	// 		heighInfo += w.CalculateWave(pos, time, lod);
	// 	}

	// 	if(heighInfo.y == 0){
	// 		points[pointId] = 0f;
	// 	}
	// 	else{
	// 		points[pointId] = heighInfo.x / heighInfo.y;
	// 	}

	// 	int beginX = Mathf.RoundToInt(pos.x * heightMapRatio);
	// 	int endX = Mathf.RoundToInt((pos.x + 1) * heightMapRatio);

	// 	int beginY = Mathf.RoundToInt(pos.y * heightMapRatio);
	// 	int endY = Mathf.RoundToInt((pos.y + 1) * heightMapRatio);
	// 	for(int i = beginX; i < endX; i++){
	// 		for(int j = beginY; j < endY; j++){
	// 			float colorHeight = (Mathf.Clamp(points[pointId], -maxWaveHeight, maxWaveHeight) / maxWaveHeight) + 0.5f;
	// 			heigthMap[i * heightMapSize + j] = new Color(colorHeight, colorHeight, colorHeight, 1f);
	// 			// heightMapTexture.SetPixel(i, j, new Color(colorHeight, colorHeight, colorHeight, 1f));
	// 		}
	// 	}
	// }

	

	public void CreateZone(){
		waves.Add(Wave.CreateZone(new Vector2(0, 0), zoneAmplitude, zoneWaveLength, zonePeriod, time));
	}

	public void CreateImpact(Vector3 p){
		CreateImpact(p, impactAmplitude, impactWaveLength, impactPeriod, distanceDigress, timeDigress, timeout);
	}

	public void CreateImpact(Vector3 p, float amplitude, float length, float period, float distanceDigress, float timeDigress, float timeout){
		float iFloat = ((p.x / ratio) + halfLod) - selfTransform.position.x;
		float jFloat = ((p.z / ratio) + halfLod) - selfTransform.position.z;

		waves.Add(Wave.CreateImpact(new Vector2(iFloat, jFloat), amplitude, length, period, time, distanceDigress, timeDigress, timeout));
	}

	private void OnDrawGizmos() {
		if(!displayGizmos){
			return ;
		}

		int lod = 32 * ((int) Mathf.Pow(2, lodPower));

		if(points.Length != (lod * lod)){
			return ;
		}

		Transform seaTransform = GetComponent<Transform>();
		float halfLod = ((float) lod) * 0.5f;
		Gizmos.color = gizmosColor;
		for(int i = 0; i < lod; i++){
			for(int j = 0; j < lod; j++){
				if(i < (lod - 1)){
					Gizmos.DrawLine(CalculateRealPosition(i, j, lod, halfLod, seaTransform), CalculateRealPosition(i + 1, j, lod, halfLod, seaTransform));
				}

				if(j < (lod - 1)){
					Gizmos.DrawLine(CalculateRealPosition(i, j, lod, halfLod, seaTransform), CalculateRealPosition(i, j + 1, lod, halfLod, seaTransform));
				}
			}
		}
	}

	public TransformInfo GetTransformInfo(Vector2 position, float yAngle){
		float minX = GetX(0);
		float maxX = GetX(lod - 1);
		float minZ = GetZ(0);
		float maxZ = GetZ(lod - 1);

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
		result.normal = GetNormal(info, result.position, result.position.y, yAngle);

		return result;
	}

	private Vector3 CalculateRealPosition(int i, int j){
		return CalculateRealPosition(i, j, lod, halfLod, selfTransform);
	}

	private Vector3 CalculateRealPosition(int i, int j, int lod, float halfLod, Transform seaTransform){
		float x = GetX(i, halfLod, seaTransform);
		float y = points[i * lod + j];
		float z = GetZ(j, halfLod, seaTransform);
		return new Vector3(x, y, z);
	}

	private float GetHeight(HeightInfo info){
		float result;

		result = info.coeff.x * points[info.i.y * lod + info.j.y];
		result += info.coeff.y * points[info.i.x * lod + info.j.y];
		result += info.coeff.z * points[info.i.y * lod + info.j.x];
		result += info.coeff.w * points[info.i.x * lod + info.j.x];

		return result / (info.coeff.x + info.coeff.y + info.coeff.z + info.coeff.w);
	}

	private Vector3 Vector3Abs(Vector3 vector){
		vector.x = Mathf.Abs(vector.x);
		vector.y = Mathf.Abs(vector.y);
		vector.z = Mathf.Abs(vector.z);
		return vector;
	}

	private Vector3 GetNormal(HeightInfo info, Vector3 position, float height, float yAngle)
    {
		Vector3 result;

        result = info.coeff.x * normales[info.i.x * lod + info.j.x];
        result += info.coeff.y * normales[info.i.y * lod + info.j.x];
        result += info.coeff.z * normales[info.i.x * lod + info.j.y];
        result += info.coeff.w * normales[info.i.y * lod + info.j.y];

        result /= (info.coeff.x + info.coeff.y + info.coeff.z + info.coeff.w);

        result.y *= -1;
        result.x *= -1;

        Debug.DrawRay(position, result * 5f);

        return result;

        {
            // Vector3 a = new Vector3(0, points[info.i.x * lod + info.j.x], 0);
            // Vector3 b = new Vector3(1, points[info.i.y * lod + info.j.x], 0);
            // Vector3 c = new Vector3(0, points[info.i.x * lod + info.j.y], 1);
            // Vector3 d = new Vector3(1, points[info.i.y * lod + info.j.y], 1);

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
        }

        {
            // float a = points[info.i.x * lod + info.j.x];
            // float b = points[info.i.y * lod + info.j.x];
            // float c = points[info.i.x * lod + info.j.y];
            // float d = points[info.i.y * lod + info.j.y];

            // if(yAngle < 0){
            // 	yAngle = 360 - yAngle;
            // }

            // while(yAngle > 90){
            // 	float temp = a;
            // 	a = c;
            // 	c = d;
            // 	d = b;
            // 	b = temp;
            // 	yAngle -= 90;
            // }



            // float ab = Mathf.Abs(a-b);
            // float cd = Mathf.Abs(c-d);
            // float ac = Mathf.Abs(a-c);
            // float bd = Mathf.Abs(b-d);

            // float diffJ;
            // float diffI;

            // float signI;
            // float signJ;

            // if(ab > cd){
            // 	diffJ = ab;
            // 	signJ = Mathf.Sign(b-a);
            // }
            // else{
            // 	diffJ = cd;
            // 	signJ = Mathf.Sign(d-c);
            // }

            // if(ac > bd){
            // 	diffI = ac;
            // 	signI = Mathf.Sign(c-a);
            // }
            // else{
            // 	diffI = bd; 
            // 	signI = Mathf.Sign(d-b);
            // }

            // float x = - signI * Vector2.Angle(new Vector2(1, 0), new Vector2(1, diffI));
            // float z = signJ * Vector2.Angle(new Vector2(1, 0), new Vector2(1, diffJ));

            // result = new Vector3(x, 0, z);

            // return result;
        }
	}

	private HeightInfo GetHeightInfo(float x, float z){
		HeightInfo result = new HeightInfo();
		float iFloat = ((x / ratio) + halfLod) - selfTransform.position.x;
		float jFloat = ((z / ratio) + halfLod) - selfTransform.position.z;

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

	private float GetX(int i, float halfLod, Transform seaTransform){
		return seaTransform.position.x + ((((float) i) - halfLod) * ratio);
	}

	private float GetZ(int j){
		return GetZ(j, halfLod, selfTransform);
	}

	private float GetZ(int j, float halfLod, Transform seaTransform){
		return seaTransform.position.z + ((((float) j) - halfLod) * ratio);
	}

	private void OnDestroy() {
		optionBuffer.Dispose();
		pointBuffer.Dispose();
		normaleBuffer.Dispose();
	}
}
