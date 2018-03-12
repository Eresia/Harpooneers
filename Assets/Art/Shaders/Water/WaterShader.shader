// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Water/WaterShader"
{
	Properties
	{
		[Header(Refraction)]
		_ChromaticAberration("Chromatic Aberration", Range( 0 , 0.3)) = 0.1
		_Normal01("Normal01", 2D) = "bump" {}
		_Normal02("Normal02", 2D) = "bump" {}
		_NormalStrenght("NormalStrenght", Range( 0 , 1)) = 0.5
		_NormalTiling("NormalTiling", Range( 0 , 10)) = 0
		_WaveSpeed("WaveSpeed", Range( 0 , 1.5)) = 0.1
		_Smoothness("Smoothness", Range( 0 , 1)) = 2
		_Opacity("Opacity", Range( 0 , 1)) = 0
		_ShallowWaterDistance("ShallowWaterDistance", Range( 0 , 1)) = 0
		_FarDeepDistance("FarDeepDistance", Range( 0 , 1)) = 0
		_MidWaterDistance("MidWaterDistance", Range( 0 , 1)) = 0
		_OverallDepth("OverallDepth", Range( 0 , 3)) = 0
		_ColorChange("ColorChange", Range( 0 , 1)) = 0
		_ShallowColor1("ShallowColor1", Color) = (0.3368836,0.6120428,0.6838235,0)
		_MidWaterColor1("MidWaterColor1", Color) = (0.466317,0.4689327,0.8455882,0)
		_DeepColor1("DeepColor1", Color) = (0.466317,0.4689327,0.8455882,0)
		_FarDeepColor1("FarDeepColor1", Color) = (0.466317,0.4689327,0.8455882,0)
		_ShallowColor2("ShallowColor2", Color) = (0.3368836,0.6120428,0.6838235,0)
		_MidWaterColor2("MidWaterColor2", Color) = (0.06060771,0.4338235,0.155842,0)
		_DeepColor2("DeepColor2", Color) = (0.06060771,0.4338235,0.155842,0)
		_FarDeepColor2("FarDeepColor2", Color) = (0.06060771,0.4338235,0.155842,0)
		_ShallowColor3("ShallowColor3", Color) = (0.3368836,0.6120428,0.6838235,0)
		_MidWaterColor3("MidWaterColor3", Color) = (0,0,0,0)
		_DeepColor3("DeepColor3", Color) = (0,0,0,0)
		_FarDeepColor3("FarDeepColor3", Color) = (0,0,0,0)
		_ShallowColor4("ShallowColor4", Color) = (0.3368836,0.6120428,0.6838235,0)
		_MidWaterColor4("MidWaterColor4", Color) = (0,0,0,0)
		_DeepColor4("DeepColor4", Color) = (0,0,0,0)
		_FarDeepColor4("FarDeepColor4", Color) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		GrabPass{ }
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma multi_compile _ALPHAPREMULTIPLY_ON
		#pragma surface surf Standard keepalpha finalcolor:RefractionF noshadow exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
			float3 worldPos;
		};

		uniform float _NormalStrenght;
		uniform sampler2D _Normal01;
		uniform float _WaveSpeed;
		uniform float _NormalTiling;
		uniform sampler2D _Normal02;
		uniform float _ColorChange;
		uniform float4 _ShallowColor4;
		uniform float4 _ShallowColor3;
		uniform float4 _ShallowColor2;
		uniform float4 _ShallowColor1;
		uniform float4 _MidWaterColor4;
		uniform float4 _MidWaterColor3;
		uniform float4 _MidWaterColor2;
		uniform float4 _MidWaterColor1;
		uniform float4 _DeepColor4;
		uniform float4 _DeepColor3;
		uniform float4 _DeepColor2;
		uniform float4 _DeepColor1;
		uniform float4 _FarDeepColor4;
		uniform float4 _FarDeepColor3;
		uniform float4 _FarDeepColor2;
		uniform float4 _FarDeepColor1;
		uniform sampler2D _CameraDepthTexture;
		uniform float _OverallDepth;
		uniform float _FarDeepDistance;
		uniform float _MidWaterDistance;
		uniform float _ShallowWaterDistance;
		uniform float _Smoothness;
		uniform float _Opacity;
		uniform sampler2D _GrabTexture;
		uniform float _ChromaticAberration;

		inline float4 Refraction( Input i, SurfaceOutputStandard o, float indexOfRefraction, float chomaticAberration ) {
			float3 worldNormal = o.Normal;
			float4 screenPos = i.screenPos;
			#if UNITY_UV_STARTS_AT_TOP
				float scale = -1.0;
			#else
				float scale = 1.0;
			#endif
			float halfPosW = screenPos.w * 0.5;
			screenPos.y = ( screenPos.y - halfPosW ) * _ProjectionParams.x * scale + halfPosW;
			#if SHADER_API_D3D9 || SHADER_API_D3D11
				screenPos.w += 0.00000000001;
			#endif
			float2 projScreenPos = ( screenPos / screenPos.w ).xy;
			float3 worldViewDir = normalize( UnityWorldSpaceViewDir( i.worldPos ) );
			float3 refractionOffset = ( ( ( ( indexOfRefraction - 1.0 ) * mul( UNITY_MATRIX_V, float4( worldNormal, 0.0 ) ) ) * ( 1.0 / ( screenPos.z + 1.0 ) ) ) * ( 1.0 - dot( worldNormal, worldViewDir ) ) );
			float2 cameraRefraction = float2( refractionOffset.x, -( refractionOffset.y * _ProjectionParams.x ) );
			float4 redAlpha = tex2D( _GrabTexture, ( projScreenPos + cameraRefraction ) );
			float green = tex2D( _GrabTexture, ( projScreenPos + ( cameraRefraction * ( 1.0 - chomaticAberration ) ) ) ).g;
			float blue = tex2D( _GrabTexture, ( projScreenPos + ( cameraRefraction * ( 1.0 + chomaticAberration ) ) ) ).b;
			return float4( redAlpha.r, green, blue, redAlpha.a );
		}

		void RefractionF( Input i, SurfaceOutputStandard o, inout fixed4 color )
		{
			#ifdef UNITY_PASS_FORWARDBASE
			float temp_output_123_0 = _Opacity;
			color.rgb = color.rgb + Refraction( i, o, temp_output_123_0, _ChromaticAberration ) * ( 1 - color.a );
			color.a = 1;
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Normal = float3(0,0,1);
			float2 temp_cast_0 = (_NormalTiling).xx;
			float2 uv_TexCoord12 = i.uv_texcoord * temp_cast_0 + float2( 0,0 );
			float2 panner13 = ( uv_TexCoord12 + 1.0 * _Time.y * ( ( ( sin( _Time.y ) * 0.0005 ) + float2( 0.25,0.1 ) ) * _WaveSpeed ));
			float2 temp_cast_1 = (_NormalTiling).xx;
			float2 uv_TexCoord39 = i.uv_texcoord * temp_cast_1 + float2( 1.37,0 );
			float2 panner16 = ( uv_TexCoord39 + 1.0 * _Time.y * ( ( ( cos( _Time.y ) * 0.0005 ) + float2( -0.5,-0.1 ) ) * _WaveSpeed ));
			o.Normal = BlendNormals( UnpackScaleNormal( tex2D( _Normal01, panner13 ) ,_NormalStrenght ) , UnpackScaleNormal( tex2D( _Normal02, panner16 ) ,_NormalStrenght ) );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth73 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth73 = abs( ( screenDepth73 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _OverallDepth ) );
			float temp_output_7_0 = abs( distanceDepth73 );
			float clampResult85 = clamp( ( temp_output_7_0 * (0.1 + (_FarDeepDistance - 0.0) * (6.0 - 0.1) / (1.0 - 0.0)) ) , 0.0 , 1.0 );
			float4 lerpResult81 = lerp( (( _ColorChange >= 0.75 && _ColorChange <= 1.0 ) ? _DeepColor4 :  (( _ColorChange >= 0.5 && _ColorChange <= 0.75 ) ? _DeepColor3 :  (( _ColorChange >= 0.25 && _ColorChange <= 0.5 ) ? _DeepColor2 :  _DeepColor1 ) ) ) , (( _ColorChange >= 0.75 && _ColorChange <= 1.0 ) ? _FarDeepColor4 :  (( _ColorChange >= 0.5 && _ColorChange <= 0.75 ) ? _FarDeepColor3 :  (( _ColorChange >= 0.25 && _ColorChange <= 0.5 ) ? _FarDeepColor2 :  _FarDeepColor1 ) ) ) , clampResult85);
			float clampResult119 = clamp( _MidWaterDistance , _FarDeepDistance , _ShallowWaterDistance );
			float clampResult43 = clamp( ( temp_output_7_0 * (0.1 + (clampResult119 - 0.0) * (6.0 - 0.1) / (1.0 - 0.0)) ) , 0.0 , 1.0 );
			float4 lerpResult41 = lerp( (( _ColorChange >= 0.75 && _ColorChange <= 1.0 ) ? _MidWaterColor4 :  (( _ColorChange >= 0.5 && _ColorChange <= 0.75 ) ? _MidWaterColor3 :  (( _ColorChange >= 0.25 && _ColorChange <= 0.5 ) ? _MidWaterColor2 :  _MidWaterColor1 ) ) ) , lerpResult81 , clampResult43);
			float clampResult118 = clamp( _ShallowWaterDistance , _MidWaterDistance , 1.0 );
			float clampResult30 = clamp( ( temp_output_7_0 * (0.1 + (clampResult118 - 0.0) * (6.0 - 0.1) / (1.0 - 0.0)) ) , 0.0 , 1.0 );
			float4 lerpResult11 = lerp( (( _ColorChange >= 0.75 && _ColorChange <= 1.0 ) ? _ShallowColor4 :  (( _ColorChange >= 0.5 && _ColorChange <= 0.75 ) ? _ShallowColor3 :  (( _ColorChange >= 0.25 && _ColorChange <= 0.5 ) ? _ShallowColor2 :  _ShallowColor1 ) ) ) , lerpResult41 , clampResult30);
			o.Albedo = lerpResult11.rgb;
			o.Smoothness = _Smoothness;
			float temp_output_123_0 = _Opacity;
			o.Alpha = temp_output_123_0;
			o.Normal = o.Normal + 0.00001 * i.screenPos * i.worldPos;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14101
7;29;1906;1004;6163.202;2594.166;5.065834;True;False
Node;AmplifyShaderEditor.TimeNode;35;-3160.134,848.73;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;74;-4010.621,-159.8276;Float;False;Property;_OverallDepth;OverallDepth;11;0;Create;0;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;80;-2584.795,-1281.11;Float;False;Property;_DeepColor1;DeepColor1;15;0;Create;0.466317,0.4689327,0.8455882,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;103;-2566.479,-1487.928;Float;False;Property;_DeepColor2;DeepColor2;19;0;Create;0.06060771,0.4338235,0.155842,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CosOpNode;40;-2840.782,998.547;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;33;-2853.557,833.881;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;9;-3523.669,-996.127;Float;False;Property;_FarDeepColor1;FarDeepColor1;16;0;Create;0.466317,0.4689327,0.8455882,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;112;-3521.216,-1248.399;Float;False;Property;_FarDeepColor2;FarDeepColor2;20;0;Create;0.06060771,0.4338235,0.155842,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;73;-3707.621,-153.8276;Float;False;True;1;0;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-3034.607,-2963.865;Float;False;Property;_ColorChange;ColorChange;12;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-3372.555,171.8355;Float;False;Property;_ShallowWaterDistance;ShallowWaterDistance;8;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-3392.844,-354.0212;Float;False;Property;_MidWaterDistance;MidWaterDistance;10;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-3275.045,-688.2043;Float;False;Property;_FarDeepDistance;FarDeepDistance;9;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;53;-2741.438,1218.173;Float;False;Constant;_Vector1;Vector 1;7;0;Create;-0.5,-0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TFHCCompareWithRange;109;-3235.867,-1114.153;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.25;False;2;FLOAT;0.5;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;105;-2554.248,-1680.877;Float;False;Property;_DeepColor3;DeepColor3;23;0;Create;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;50;-2707.125,652.4383;Float;False;Constant;_Vector0;Vector 0;7;0;Create;0.25,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ColorNode;42;-1684.765,-996.19;Float;False;Property;_MidWaterColor1;MidWaterColor1;14;0;Create;0.466317,0.4689327,0.8455882,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-2657.189,960.067;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0005;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;7;-2812.376,-155.6158;Float;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-2636.189,844.0674;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0005;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;104;-2300.157,-1401.952;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.25;False;2;FLOAT;0.5;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;113;-3509.419,-1455.084;Float;False;Property;_FarDeepColor3;FarDeepColor3;24;0;Create;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;83;-2690.685,-639.6464;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.1;False;4;FLOAT;6.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;100;-1679.815,-1180.917;Float;False;Property;_MidWaterColor2;MidWaterColor2;18;0;Create;0.06060771,0.4338235,0.155842,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;119;-2927.169,-362.8975;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;91;-838.5885,-761.8896;Float;False;Property;_ShallowColor2;ShallowColor2;17;0;Create;0.3368836,0.6120428,0.6838235,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-2376.442,-432.3426;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;5.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-854.2129,-573.4636;Float;False;Property;_ShallowColor1;ShallowColor1;13;0;Create;0.3368836,0.6120428,0.6838235,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;65;-2288.133,952.8138;Float;False;Property;_WaveSpeed;WaveSpeed;5;0;Create;0.1;0;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;101;-1667.583,-1373.865;Float;False;Property;_MidWaterColor3;MidWaterColor3;22;0;Create;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCCompareWithRange;110;-3000.049,-1118.198;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.5;False;2;FLOAT;0.75;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;107;-2122.636,-1544.24;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.5;False;2;FLOAT;0.75;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;114;-3504.005,-1654.799;Float;False;Property;_FarDeepColor4;FarDeepColor4;28;0;Create;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;60;-2381.848,1074.622;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT2;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;97;-1413.492,-1094.941;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.25;False;2;FLOAT;0.5;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;-2367.189,764.0674;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT2;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;118;-2955.516,73.30389;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;45;-2688.941,-362.1417;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.1;False;4;FLOAT;6.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;106;-2539.706,-1950.809;Float;False;Property;_DeepColor4;DeepColor4;27;0;Create;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;79;-2252.169,579.9909;Float;False;Property;_NormalTiling;NormalTiling;4;0;Create;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-1880.046,638.9815;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCCompareWithRange;96;-505.414,-760.8149;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.25;False;2;FLOAT;0.5;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;32;-2841.168,183.3163;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.1;False;4;FLOAT;6.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-2017.657,757.7512;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0.0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;39;-1886.558,918.1344;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2.3,2.3;False;1;FLOAT2;1.37,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-1968.157,1056.251;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0.0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;102;-1663.145,-1564.054;Float;False;Property;_MidWaterColor4;MidWaterColor4;26;0;Create;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCCompareWithRange;111;-2724.528,-1139.452;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.75;False;2;FLOAT;1.0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;108;-1931.005,-1670.359;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.75;False;2;FLOAT;1.0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;98;-1235.971,-1237.228;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.5;False;2;FLOAT;0.75;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;85;-1966.399,-410.6267;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-2374.699,-154.8384;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;5.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;90;-844.2222,-988.4034;Float;False;Property;_ShallowColor3;ShallowColor3;21;0;Create;0.3368836,0.6120428,0.6838235,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCCompareWithRange;99;-1044.341,-1363.347;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.75;False;2;FLOAT;1.0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-2474.87,135.2867;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;5.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;13;-1587.242,777.7659;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0.05;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;87;-842.7126,-1187.511;Float;False;Property;_ShallowColor4;ShallowColor4;25;0;Create;0.3368836,0.6120428,0.6838235,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCCompareWithRange;95;-338.4456,-903.7869;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.5;False;2;FLOAT;0.75;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;81;-1695.529,-696.1373;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1536.467,1376.826;Float;False;Property;_NormalStrenght;NormalStrenght;3;0;Create;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;16;-1571.926,991.9102;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0.1;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;43;-1964.656,-133.1228;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1085.242,815.5317;Float;True;Property;_Normal01;Normal01;1;0;Create;Assets/Normal01.png;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;41;-869.0328,-347.3877;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;94;-152.3373,-1013.751;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.75;False;2;FLOAT;1.0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;3;-1082.642,1097.631;Float;True;Property;_Normal02;Normal02;2;0;Create;Assets/Normal02.png;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;30;-2123.139,161.4884;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-366.6011,102.6297;Float;False;Property;_Smoothness;Smoothness;6;0;Create;2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;17;-729.2349,864.8979;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;11;-230.9935,-61.233;Float;False;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;123;-360.1958,200.343;Float;False;Property;_Opacity;Opacity;7;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Water/WaterShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Translucent;0.5;True;False;0;False;Opaque;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;0;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;0;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;40;0;35;2
WireConnection;33;0;35;2
WireConnection;73;0;74;0
WireConnection;109;0;89;0
WireConnection;109;3;112;0
WireConnection;109;4;9;0
WireConnection;62;0;40;0
WireConnection;7;0;73;0
WireConnection;56;0;33;0
WireConnection;104;0;89;0
WireConnection;104;3;103;0
WireConnection;104;4;80;0
WireConnection;83;0;82;0
WireConnection;119;0;46;0
WireConnection;119;1;82;0
WireConnection;119;2;31;0
WireConnection;84;0;7;0
WireConnection;84;1;83;0
WireConnection;110;0;89;0
WireConnection;110;3;113;0
WireConnection;110;4;109;0
WireConnection;107;0;89;0
WireConnection;107;3;105;0
WireConnection;107;4;104;0
WireConnection;60;0;62;0
WireConnection;60;1;53;0
WireConnection;97;0;89;0
WireConnection;97;3;100;0
WireConnection;97;4;42;0
WireConnection;58;0;56;0
WireConnection;58;1;50;0
WireConnection;118;0;31;0
WireConnection;118;1;46;0
WireConnection;45;0;119;0
WireConnection;12;0;79;0
WireConnection;96;0;89;0
WireConnection;96;3;91;0
WireConnection;96;4;10;0
WireConnection;32;0;118;0
WireConnection;59;0;58;0
WireConnection;59;1;65;0
WireConnection;39;0;79;0
WireConnection;61;0;60;0
WireConnection;61;1;65;0
WireConnection;111;0;89;0
WireConnection;111;3;114;0
WireConnection;111;4;110;0
WireConnection;108;0;89;0
WireConnection;108;3;106;0
WireConnection;108;4;107;0
WireConnection;98;0;89;0
WireConnection;98;3;101;0
WireConnection;98;4;97;0
WireConnection;85;0;84;0
WireConnection;44;0;7;0
WireConnection;44;1;45;0
WireConnection;99;0;89;0
WireConnection;99;3;102;0
WireConnection;99;4;98;0
WireConnection;27;0;7;0
WireConnection;27;1;32;0
WireConnection;13;0;12;0
WireConnection;13;2;59;0
WireConnection;95;0;89;0
WireConnection;95;3;90;0
WireConnection;95;4;96;0
WireConnection;81;0;108;0
WireConnection;81;1;111;0
WireConnection;81;2;85;0
WireConnection;16;0;39;0
WireConnection;16;2;61;0
WireConnection;43;0;44;0
WireConnection;1;1;13;0
WireConnection;1;5;18;0
WireConnection;41;0;99;0
WireConnection;41;1;81;0
WireConnection;41;2;43;0
WireConnection;94;0;89;0
WireConnection;94;3;87;0
WireConnection;94;4;95;0
WireConnection;3;1;16;0
WireConnection;3;5;18;0
WireConnection;30;0;27;0
WireConnection;17;0;1;0
WireConnection;17;1;3;0
WireConnection;11;0;94;0
WireConnection;11;1;41;0
WireConnection;11;2;30;0
WireConnection;0;0;11;0
WireConnection;0;1;17;0
WireConnection;0;4;66;0
WireConnection;0;8;123;0
WireConnection;0;9;123;0
ASEEND*/
//CHKSM=4F499224676A036DA270F0513D5FC044DCC8BDE6