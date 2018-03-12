Shader "Custom/Test_Surface_Sea" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType" = "Opaque" }
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		struct Input {
			float2 uv_NormaleMap;
		};

		sampler2D _HeightMap;
		float _MaxWaveHeight;

		void vert (inout appdata_full v) {
			v.vertex.y = (tex2Dlod (_HeightMap, float4(v.texcoord.xy, 0, 0)).r - 0.5) * _MaxWaveHeight;
		}

		
		sampler2D _NormaleMap;
		float4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = _Color;
			o.Normal = UnpackNormal (tex2D (_NormaleMap, IN.uv_NormaleMap));
		}
      	ENDCG
	}
	FallBack "Diffuse"
}
