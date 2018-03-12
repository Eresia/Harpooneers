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

	public Renderer selfRenderer;

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

	public float waveSpeed;
	public float timeProgression;

	public float timeout;

	public Vector2 waveSize;

	public RawImage rawImage;

	private void Awake() {
		selfTransform = GetComponent<Transform>();
		lodPowPower = ((int) Mathf.Pow(2, lodPower));
		lod = 32 * lodPowPower;
		normales = new Vector3[lod * lod];
		halfLod = ((float) lod) * 0.5f;
		waves = new List<WaveOptions>();
		if(zoneAmplitude != 0){
			AddWave(Wave.CreateZone(zoneAmplitude, zoneWaveLength, zonePeriod, time));
			AddWave(Wave.CreateZoneTest(zoneAmplitude * 2, zoneWaveLength * 2, zonePeriod, time + 1));
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

		selfRenderer.material = GetComponent<Renderer>().material;
		selfRenderer.material.SetTexture("_HeightMap", normaleMapTexture);
		selfRenderer.material.SetTexture("_NormaleMap", normaleMapTexture);
		selfRenderer.material.SetBuffer("_Vertex", pointBuffer);
		selfRenderer.material.SetFloat("_MaxWaveHeight", maxWaveHeight);
		selfRenderer.material.SetInt("_VertexSize", lod);

		if(rawImage != null){
			rawImage.texture = normaleMapTexture;
		}
	}

	private void Update() {
		time += Time.deltaTime;
		bool leftClick = Input.GetMouseButtonDown(0);
		bool rightClick = Input.GetMouseButtonDown(1);
		if((GameManager.instance.actualPlayer == -1)){
			if(leftClick || rightClick){
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				// Debug.DrawRay()

				if (Physics.Raycast(ray, out hit, Mathf.Infinity, testLayer)) {
					float iFloat = ((hit.point.x / ratio) + halfLod) - selfTransform.position.x;
					float jFloat = ((hit.point.z / ratio) + halfLod) - selfTransform.position.z;
					if(leftClick){
						AddWave(Wave.CreateImpact(new Vector2(iFloat, jFloat), impactAmplitude, impactWaveLength, impactPeriod, time, waveSpeed, timeProgression, timeout));
					}
					else{
						AddWave(Wave.CreateRectImpact(new Vector2(iFloat, jFloat), waveSize, impactAmplitude, impactWaveLength, impactPeriod, time, waveSpeed, timeProgression, timeout));
					}
				}
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

			ComputeBuffer impacts = new ComputeBuffer(waveArray.Length, 16 * sizeof(float));
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
	}

	public void AddWave(WaveOptions wave){
		waves.Add(wave);
	}

	public void CreateImpact(Vector3 position){
		float iFloat = ((position.x / ratio) + halfLod) - selfTransform.position.x;
		float jFloat = ((position.z / ratio) + halfLod) - selfTransform.position.z;

		AddWave(Wave.CreateImpact(new Vector2(iFloat, jFloat), impactAmplitude, impactWaveLength, impactPeriod, time, waveSpeed, timeProgression, timeout));
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

        // Debug.DrawRay(position, result * 5f);

        return result;
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
