using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
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

	[NonSerialized]
	private float[] points;

	[NonSerialized]
	private Vector3[] normales;

	public WaveManager waveManager {get; private set;}

	public int ZoneWaveId {get; private set;}

	private Transform selfTransform;

	private int lod;

	private float halfLod;

	private RenderTexture heightMap;

	private RenderTexture normaleMap;

	private ComputeBuffer optionBuffer;

	private ComputeBuffer pointBuffer;

	private ComputeBuffer normaleBuffer;

	private FrameOptions[] frameOptions;

	private int lodPowPower;

	[Space]

	public bool displayGizmos;
	public bool displayNormal;
	public Color gizmosColor;

	public LayerMask testLayer;


	[Space]

	[Header("Zone Waves options")]

	public float zoneAmplitude;

	public float zoneRotation;

	public float zoneWaveLength;

	public float zonePeriod;

	[Space]

	[Header("Impact Waves options")]

	[Range(0, 1000)]
	public float impactAmplitude;

	[Range(0, 1000)]
	public float impactRadius;

	[Range(0, 1000)]
	public float vortexSmooth;

	[Range(0, 1000)]
	public float impactWaveLength;

	[Range(0, 1000)]
	public float impactPeriod;

	[Range(0, 1000)]
	public float timeProgression;

	[Range(0, 1000)]
	public float timeout;

	public float waveRotation;

	public Vector2 waveSize;

	public RawImage rawImage;

	private int waveVortexId = -1;

	private bool canBeginUpdate;

	private void Awake()
    {
		GameManager.instance.ground = this;
        waveManager = new WaveManager();
        ZoneWaveId = waveManager.CreateZone(0f, zoneRotation, zoneWaveLength, zonePeriod);

        // StartCoroutine(AwakeCoroutine());
		AwakeCoroutine();
	}

	// private IEnumerator AwakeCoroutine()
	private void AwakeCoroutine()
    {
		selfTransform = GetComponent<Transform>();
		lodPowPower = ((int) Mathf.Pow(2, lodPower));
		lod = 32 * lodPowPower;
		// normales = new Vector3[lod * lod];
		halfLod = ((float) lod) * 0.5f;

		points = new float[lod * lod];
		normales = new Vector3[lod * lod];

		// yield return null;

		for(int i = 0; i < lod; i++){
			for(int j = 0; j < lod; j++){
				points[i*lod + j] = selfTransform.position.y;
				normales[i*lod + j] = new Vector3(0f, 1f, 0f);
			}
			// if(i%1000 == 0){
			// 	yield return null;
			// }
		}
        
		// waveManager.CreateZoneTest(zoneAmplitude * 2, zoneWaveLength * 2, zonePeriod);

		int heightMapLod = 32 * ((int) Mathf.Pow(2, heigtMapPower));

		heightMap = new RenderTexture(heightMapLod, heightMapLod, 24, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
		heightMap.name = "HeightMap";
		heightMap.enableRandomWrite = true;
		heightMap.Create();

		normaleMap = new RenderTexture(heightMapLod, heightMapLod, 24);
		normaleMap.name = "NormaleMap";
		normaleMap.enableRandomWrite = true;
		normaleMap.Create();

		frameOptions = new FrameOptions[1];
		frameOptions[0] = new FrameOptions();
		frameOptions[0].maxWaveHeight = maxWaveHeight;
		frameOptions[0].heigtMapRatio = (uint) (heightMapLod / lod);
		frameOptions[0].ratio = (lodPowPower*ratio) / 4;
		// frameOptions[0].ratio = 0.5f;

		// yield return null;

		pointBuffer = new ComputeBuffer(points.Length, sizeof(float));
		normaleBuffer = new ComputeBuffer(normales.Length, 3 * sizeof(float));

		optionBuffer = new ComputeBuffer(1, 8 * sizeof(float));

		int pointKernel = seaCompute.FindKernel("CalculatePoint");
		int normaleKernel = seaCompute.FindKernel("CalculateNormal");

		seaCompute.SetBuffer(pointKernel, "Options", optionBuffer);
		seaCompute.SetBuffer(pointKernel, "Result", pointBuffer);
		seaCompute.SetTexture(pointKernel, "HeightMap", heightMap);

		seaCompute.SetBuffer(normaleKernel, "Options", optionBuffer);
		seaCompute.SetBuffer(normaleKernel, "Result", pointBuffer);
		seaCompute.SetBuffer(normaleKernel, "Normales", normaleBuffer);
		seaCompute.SetTexture(normaleKernel, "NormaleMap", normaleMap);

		// yield return null;

		selfRenderer.material = GetComponent<Renderer>().material;
		selfRenderer.material.SetTexture("_InputHeight", heightMap);
		selfRenderer.material.SetTexture("_InputNormal", normaleMap);
		selfRenderer.material.SetBuffer("_Vertex", pointBuffer);
		selfRenderer.material.SetFloat("_MaxWaveHeight", maxWaveHeight);
		selfRenderer.material.SetFloat("_HeightCoeff", maxWaveHeight * 2);
		selfRenderer.material.SetInt("_VertexSize", lod);

		if(rawImage != null){
			rawImage.texture = heightMap;
		}
		canBeginUpdate = true;
	}

	private void Update() {
		if(!canBeginUpdate){
			return ;
		}

		if(Input.GetKeyDown(KeyCode.Space)){
			RenderTexture currentActiveRT = RenderTexture.active;
			RenderTexture.active = heightMap;
			Texture2D tex = new Texture2D(heightMap.width, heightMap.height);
			tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
			var bytes = tex.EncodeToPNG();
			System.IO.File.WriteAllBytes("TEST", bytes);
			UnityEngine.Object.Destroy(tex);
			RenderTexture.active = currentActiveRT;
		}

		bool leftClick = Input.GetMouseButtonDown(0);
		bool rightClick = Input.GetMouseButtonDown(1);
		if((GameManager.instance.actualPlayer == -1)){
			if(leftClick || rightClick){
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				// Debug.DrawRay()

				if (Physics.Raycast(ray, out hit, Mathf.Infinity, testLayer)) {
					float iFloat = ((hit.point.x / (4*ratio / lodPowPower)) + halfLod) - selfTransform.position.x;
					float jFloat = ((hit.point.z / (4*ratio / lodPowPower)) + halfLod) - selfTransform.position.z;
					if(leftClick){
						// AddWave(Wave.CreateImpact(new Vector2(iFloat, jFloat), impactAmplitude, impactWaveLength, impactPeriod, time, waveSpeed, timeProgression, timeout));
						waveManager.CreateTraceImpact(new Vector2(iFloat, jFloat), waveSize.x, waveRotation, impactAmplitude, impactWaveLength, impactPeriod, timeProgression, timeout);
					}
					else{
						waveVortexId = waveManager.CreateVortex(new Vector2(iFloat, jFloat), impactAmplitude, impactRadius, vortexSmooth, impactWaveLength, impactPeriod, timeProgression, timeout);
						// AddWave(Wave.CreateRectImpact(new Vector2(iFloat, jFloat), waveSize, impactAmplitude, impactWaveLength, impactPeriod, time, waveSpeed, timeProgression, timeout));
					}
				}
			}

			if(waveManager.Waves.ContainsKey(waveVortexId)){
				WaveOptions vortex = waveManager.Waves[waveVortexId];
				impactAmplitude += Input.GetAxis("Mouse ScrollWheel");
				vortex.amplitude = impactAmplitude;
				vortex.radius = impactRadius;
				vortex.smooth = vortexSmooth;
				vortex.waveNumber = (2 * Mathf.PI) / impactWaveLength;
				vortex.angularFrequency = (2 * Mathf.PI) / impactPeriod;
				vortex.progression = timeProgression;
				waveManager.ChangeWave(waveVortexId, vortex);
			}
			
		}

		waveManager.IncrementTime(Time.deltaTime);
		
		frameOptions[0].time = waveManager.ActualTime;
		frameOptions[0].nbWaves = (uint) waveManager.Waves.Count;
		frameOptions[0].lod = (uint) lod;
		optionBuffer.SetData(frameOptions);

		ComputeBuffer impacts = new ComputeBuffer(waveManager.Waves.Count, 16 * sizeof(float));
		impacts.SetData(waveManager.Waves.Values.ToArray());

		pointBuffer.SetData(points);

		int pointKernel = seaCompute.FindKernel("CalculatePoint");

		seaCompute.SetBuffer(pointKernel, "Impacts", impacts);

		seaCompute.Dispatch(pointKernel, lodPowPower, lodPowPower, 1);

		pointBuffer.GetData(points);
		normaleBuffer.GetData(normales);

		impacts.Dispose();

		waveManager.RefreshWaves();

		seaCompute.Dispatch(seaCompute.FindKernel("CalculateNormal"), lodPowPower, lodPowPower, 1);
	}

	public void CreateImpact(Vector3 position){
		float iFloat = ((position.x / (4*ratio / lodPowPower)) + halfLod) - selfTransform.position.x;
		float jFloat = ((position.z / (4*ratio / lodPowPower)) + halfLod) - selfTransform.position.z;

		waveManager.CreateImpact(new Vector2(iFloat, jFloat), impactAmplitude, impactRadius, impactWaveLength, impactPeriod, timeProgression, timeout);
	}

	public Vector2 GetSeaPosition(Vector3 position){
		float iFloat = ((position.x / (4*ratio / lodPowPower)) + halfLod) - selfTransform.position.x;
		float jFloat = ((position.z / (4*ratio / lodPowPower)) + halfLod) - selfTransform.position.z;

		return new Vector2(iFloat, jFloat);
	}

	private void OnDrawGizmos() {
		if(!displayGizmos){
			return ;
		}

		if((points == null) || (normales == null)){
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
				if(displayNormal){
					Gizmos.DrawRay(CalculateRealPosition(i, j, lod, halfLod, seaTransform), normales[i * lod + j]);
				}
				else{
					if(i < (lod - 1)){
						Gizmos.DrawLine(CalculateRealPosition(i, j, lod, halfLod, seaTransform), CalculateRealPosition(i + 1, j, lod, halfLod, seaTransform));

					}

					if(j < (lod - 1)){
						Gizmos.DrawLine(CalculateRealPosition(i, j, lod, halfLod, seaTransform), CalculateRealPosition(i, j + 1, lod, halfLod, seaTransform));
					}
				}
			}
		}
	}

	public TransformInfo GetTransformInfo(Vector3 position){
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

		if(position.z < minZ){
			result.position.z = minZ;
		}
		else if(position.z > maxZ){
			result.position.z = maxZ;
		}
		else{
			result.position.z = position.y;
		}

		HeightInfo info = GetHeightInfo(result.position.x, result.position.z);
		result.position.y = GetHeight(info);
		result.normal = GetNormal(info, result.position, result.position.y);

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

	private Vector3 GetNormal(HeightInfo info, Vector3 position, float height)
    {
		Vector3 result;

        result = info.coeff.x * normales[info.i.x * lod + info.j.x];
        result += info.coeff.y * normales[info.i.y * lod + info.j.x];
        result += info.coeff.z * normales[info.i.x * lod + info.j.y];
        result += info.coeff.w * normales[info.i.y * lod + info.j.y];

        result /= (info.coeff.x + info.coeff.y + info.coeff.z + info.coeff.w);

        // result.y *= -1;
        // result.x *= -1;

        // Debug.DrawRay(position, normales[info.i.x * lod + info.j.x] * 5f);
		// Debug.DrawRay(position, normales[info.i.y * lod + info.j.x] * 5f);
		// Debug.DrawRay(position, normales[info.i.x * lod + info.j.y] * 5f);
		// Debug.DrawRay(position, normales[info.i.y * lod + info.j.y] * 5f);

        return result;
	}

	private HeightInfo GetHeightInfo(float x, float z){
		HeightInfo result = new HeightInfo();
		float iFloat = ((x / (4*ratio / lodPowPower)) + halfLod) - selfTransform.position.x;
		float jFloat = ((z / (4*ratio / lodPowPower)) + halfLod) - selfTransform.position.z;

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
		return seaTransform.position.x + ((((float) i) - halfLod) * (4*ratio / lodPowPower));
	}

	private float GetZ(int j){
		return GetZ(j, halfLod, selfTransform);
	}

	private float GetZ(int j, float halfLod, Transform seaTransform){
		return seaTransform.position.z + ((((float) j) - halfLod) * (4*ratio / lodPowPower));
	}

	private void OnDestroy() {
		if(optionBuffer != null){
			optionBuffer.Dispose();
		}
		
		if(pointBuffer != null){
			pointBuffer.Dispose();
		}
		
		if(normaleBuffer != null){
			normaleBuffer.Dispose();
		}
	}
}
