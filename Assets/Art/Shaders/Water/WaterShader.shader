// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Water/WaterShader"
{
	Properties
	{
		_InputNormal("InputNormal", 2D) = "bump" {}
		_InputHeight("InputHeight", 2D) = "white" {}
		_Normal01("Normal01", 2D) = "bump" {}
		_Normal02("Normal02", 2D) = "bump" {}
		_NormalStrenght("NormalStrenght", Range( 0 , 1)) = 0.5
		_NormalTiling1("NormalTiling1", Range( 0 , 20)) = 0
		_NormalTiling2("NormalTiling2", Range( 0 , 20)) = 0
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
		_HeightCoeff("HeightCoeff", Range( 0 , 1000)) = 1
		_HeightColorStrength("HeightColorStrength", Range( 0 , 1)) = 0
		_T_SeaFoam("T_SeaFoam", 2D) = "white" {}
		_SeaFoamMask("SeaFoamMask", 2D) = "white" {}
		_FoamTiling("FoamTiling", Range( 0 , 20)) = 0
		_SeaFoamIntensity("SeaFoamIntensity", Range( 0 , 5)) = 0
		_FoamSpeedVector("FoamSpeedVector", Vector) = (0,0,0,0)
		_FoamSpeed("FoamSpeed", Range( 0 , 0.5)) = 0.5
		_FoamDistance("FoamDistance", Range( 0 , 5)) = 0
		_WaveDeepIntensity("WaveDeepIntensity", Range( 0 , 5)) = 1.374183
		_WaveDeepDistance("WaveDeepDistance", Range( 0 , 5)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" }
		Cull Back
		CGPROGRAM
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityCG.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow exclude_path:deferred vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform sampler2D _InputNormal;
		uniform float4 _InputNormal_ST;
		uniform float _NormalStrenght;
		uniform sampler2D _Normal01;
		uniform float _WaveSpeed;
		uniform float _NormalTiling1;
		uniform sampler2D _Normal02;
		uniform float _NormalTiling2;
		uniform sampler2D _T_SeaFoam;
		uniform float2 _FoamSpeedVector;
		uniform float _FoamSpeed;
		uniform float _FoamTiling;
		uniform float _WaveDeepDistance;
		uniform sampler2D _InputHeight;
		uniform float4 _InputHeight_ST;
		uniform float _WaveDeepIntensity;
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
		uniform float _HeightColorStrength;
		uniform float _FoamDistance;
		uniform float _SeaFoamIntensity;
		uniform sampler2D _SeaFoamMask;
		uniform float _Smoothness;
		uniform float _Opacity;
		uniform float _HeightCoeff;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_InputHeight = v.texcoord * _InputHeight_ST.xy + _InputHeight_ST.zw;
			float4 tex2DNode147 = tex2Dlod( _InputHeight, float4( uv_InputHeight, 0, 0.0) );
			v.vertex.xyz += ( ( tex2DNode147.r + -0.5 ) * float3(0,0,1) * _HeightCoeff );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_InputNormal = i.uv_texcoord * _InputNormal_ST.xy + _InputNormal_ST.zw;
			float2 temp_cast_0 = (_NormalTiling1).xx;
			float2 uv_TexCoord12 = i.uv_texcoord * temp_cast_0 + float2( 0,0 );
			float2 panner13 = ( uv_TexCoord12 + 1.0 * _Time.y * ( ( ( sin( _Time.y ) * 0.0005 ) + float2( 0.25,0.1 ) ) * _WaveSpeed ));
			float2 temp_cast_1 = (_NormalTiling2).xx;
			float2 uv_TexCoord39 = i.uv_texcoord * temp_cast_1 + float2( 1.37,0 );
			float2 panner16 = ( uv_TexCoord39 + 1.0 * _Time.y * ( ( ( cos( _Time.y ) * 0.0005 ) + float2( -0.5,-0.1 ) ) * _WaveSpeed ));
			o.Normal = BlendNormals( UnpackNormal( tex2D( _InputNormal, uv_InputNormal ) ) , BlendNormals( UnpackScaleNormal( tex2D( _Normal01, panner13 ) ,_NormalStrenght ) , UnpackScaleNormal( tex2D( _Normal02, panner16 ) ,_NormalStrenght ) ) );
			float2 temp_cast_2 = (_FoamTiling).xx;
			float2 uv_TexCoord135 = i.uv_texcoord * temp_cast_2 + float2( 0,0 );
			float2 panner172 = ( uv_TexCoord135 + 1.0 * _Time.y * ( ( _FoamSpeedVector + ( sin( _Time.y ) * 0.002 ) ) * _FoamSpeed ));
			float2 uv_InputHeight = i.uv_texcoord * _InputHeight_ST.xy + _InputHeight_ST.zw;
			float4 tex2DNode147 = tex2D( _InputHeight, uv_InputHeight );
			float clampResult225 = clamp( ( _WaveDeepDistance * ( 1.0 - tex2DNode147.r ) ) , 0.0 , 1.0 );
			float lerpResult222 = lerp( 1.0 , 0.0 , ( 1.0 - ( ( 1.0 - clampResult225 ) * _WaveDeepIntensity ) ));
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth73 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float distanceDepth73 = abs( ( screenDepth73 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _OverallDepth ) );
			float temp_output_7_0 = abs( distanceDepth73 );
			float clampResult85 = clamp( ( temp_output_7_0 * (0.0 + (_FarDeepDistance - 0.0) * (6.0 - 0.0) / (1.0 - 0.0)) ) , 0.0 , 1.0 );
			float4 lerpResult81 = lerp( (( _ColorChange >= 0.75 && _ColorChange <= 1.0 ) ? _DeepColor4 :  (( _ColorChange >= 0.5 && _ColorChange <= 0.75 ) ? _DeepColor3 :  (( _ColorChange >= 0.25 && _ColorChange <= 0.5 ) ? _DeepColor2 :  _DeepColor1 ) ) ) , (( _ColorChange >= 0.75 && _ColorChange <= 1.0 ) ? _FarDeepColor4 :  (( _ColorChange >= 0.5 && _ColorChange <= 0.75 ) ? _FarDeepColor3 :  (( _ColorChange >= 0.25 && _ColorChange <= 0.5 ) ? _FarDeepColor2 :  _FarDeepColor1 ) ) ) , clampResult85);
			float clampResult119 = clamp( _MidWaterDistance , _FarDeepDistance , _ShallowWaterDistance );
			float clampResult43 = clamp( ( temp_output_7_0 * (0.1 + (clampResult119 - 0.0) * (6.0 - 0.1) / (1.0 - 0.0)) ) , 0.0 , 1.0 );
			float4 lerpResult41 = lerp( (( _ColorChange >= 0.75 && _ColorChange <= 1.0 ) ? _MidWaterColor4 :  (( _ColorChange >= 0.5 && _ColorChange <= 0.75 ) ? _MidWaterColor3 :  (( _ColorChange >= 0.25 && _ColorChange <= 0.5 ) ? _MidWaterColor2 :  _MidWaterColor1 ) ) ) , lerpResult81 , clampResult43);
			float clampResult118 = clamp( _ShallowWaterDistance , _MidWaterDistance , 1.0 );
			float clampResult30 = clamp( ( temp_output_7_0 * (0.1 + (clampResult118 - 0.0) * (6.0 - 0.1) / (1.0 - 0.0)) ) , 0.0 , 1.0 );
			float4 lerpResult11 = lerp( (( _ColorChange >= 0.75 && _ColorChange <= 1.0 ) ? _ShallowColor4 :  (( _ColorChange >= 0.5 && _ColorChange <= 0.75 ) ? _ShallowColor3 :  (( _ColorChange >= 0.25 && _ColorChange <= 0.5 ) ? _ShallowColor2 :  _ShallowColor1 ) ) ) , lerpResult41 , clampResult30);
			float clampResult163 = clamp( tex2DNode147.r , 0.0 , 1.0 );
			float clampResult168 = clamp( ( ( 1.0 - tex2DNode147.r ) * _FoamDistance ) , 0.0 , 1.0 );
			float2 uv_TexCoord236 = i.uv_texcoord * float2( 1,1 ) + float2( 0,0 );
			float2 panner235 = ( uv_TexCoord236 + 1.0 * _Time.y * float2( 0.05,0.01 ));
			float clampResult234 = clamp( ( ( ( 1.0 - clampResult168 ) * _SeaFoamIntensity ) - ( 1.0 - tex2D( _SeaFoamMask, panner235 ).r ) ) , 0.0 , 1.0 );
			float4 lerpResult133 = lerp( tex2D( _T_SeaFoam, panner172 ) , ( ( lerpResult222 * lerpResult11 ) * ( clampResult163 + _HeightColorStrength ) ) , ( 1.0 - clampResult234 ));
			o.Albedo = lerpResult133.rgb;
			o.Smoothness = _Smoothness;
			o.Alpha = _Opacity;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14101
7;29;1906;1004;2575.625;1345.869;2.045487;True;True
Node;AmplifyShaderEditor.RangedFloatNode;74;-4881.021,-210.8501;Float;False;Property;_OverallDepth;OverallDepth;13;0;Create;0;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;147;-2459.689,368.0813;Float;True;Property;_InputHeight;InputHeight;1;0;Create;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;46;-4263.246,-405.0437;Float;False;Property;_MidWaterDistance;MidWaterDistance;12;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-4145.446,-739.2268;Float;False;Property;_FarDeepDistance;FarDeepDistance;11;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;9;-4394.071,-1047.149;Float;False;Property;_FarDeepColor1;FarDeepColor1;18;0;Create;0.466317,0.4689327,0.8455882,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;228;-1969.956,30.96695;Float;False;Property;_WaveDeepDistance;WaveDeepDistance;41;0;Create;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-4353.957,121.813;Float;False;Property;_ShallowWaterDistance;ShallowWaterDistance;10;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;103;-3436.88,-1538.95;Float;False;Property;_DeepColor2;DeepColor2;21;0;Create;0.06060771,0.4338235,0.155842,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;112;-4391.618,-1299.422;Float;False;Property;_FarDeepColor2;FarDeepColor2;22;0;Create;0.06060771,0.4338235,0.155842,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;80;-3455.196,-1332.132;Float;False;Property;_DeepColor1;DeepColor1;17;0;Create;0.466317,0.4689327,0.8455882,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;89;-3843.225,-2668.656;Float;False;Property;_ColorChange;ColorChange;14;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;73;-4578.022,-204.8501;Float;False;True;1;0;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;227;-1939.369,120.0117;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;165;-1652.545,559.4342;Float;False;Constant;_Float5;Float 5;35;0;Create;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;164;-1653.845,489.234;Float;False;Constant;_Float4;Float 4;35;0;Create;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;83;-3561.086,-690.6689;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;6.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;105;-3424.649,-1731.899;Float;False;Property;_DeepColor3;DeepColor3;25;0;Create;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;166;-1881.756,-127.5912;Float;False;1;0;FLOAT;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;191;-1910.157,-269.4716;Float;False;Property;_FoamDistance;FoamDistance;39;0;Create;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;113;-4379.821,-1506.106;Float;False;Property;_FarDeepColor3;FarDeepColor3;26;0;Create;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.AbsOpNode;7;-3682.777,-206.6383;Float;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;226;-1710.762,103.0048;Float;True;2;2;0;FLOAT;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;35;-3177.961,1264.946;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;42;-2555.166,-1047.212;Float;False;Property;_MidWaterColor1;MidWaterColor1;16;0;Create;0.466317,0.4689327,0.8455882,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;119;-3797.57,-413.92;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;104;-3170.558,-1452.974;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.25;False;2;FLOAT;0.5;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;100;-2550.216,-1231.939;Float;False;Property;_MidWaterColor2;MidWaterColor2;20;0;Create;0.06060771,0.4338235,0.155842,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCCompareWithRange;109;-4106.268,-1165.175;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.25;False;2;FLOAT;0.5;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-3246.843,-483.3651;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;5.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;97;-2283.893,-1145.964;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.25;False;2;FLOAT;0.5;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TimeNode;180;-760.5388,-1202.989;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCCompareWithRange;107;-2993.037,-1595.262;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.5;False;2;FLOAT;0.75;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;190;-1637.957,-132.2351;Float;True;2;2;0;FLOAT;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;236;-911.9639,-774.0317;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;106;-3410.107,-2001.831;Float;False;Property;_DeepColor4;DeepColor4;29;0;Create;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;91;-1673.947,-928.0525;Float;False;Property;_ShallowColor2;ShallowColor2;19;0;Create;0.3368836,0.6120428,0.6838235,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;114;-4374.407,-1705.821;Float;False;Property;_FarDeepColor4;FarDeepColor4;30;0;Create;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;45;-3559.342,-413.1642;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.1;False;4;FLOAT;6.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;101;-2537.984,-1424.887;Float;False;Property;_MidWaterColor3;MidWaterColor3;24;0;Create;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;10;-1689.572,-739.6265;Float;False;Property;_ShallowColor1;ShallowColor1;15;0;Create;0.3368836,0.6120428,0.6838235,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;225;-1482.292,138.0254;Float;False;3;0;FLOAT;0,0,0,0;False;1;FLOAT;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;118;-3929.917,124.2814;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;33;-2853.557,1229.298;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;110;-3870.45,-1169.22;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.5;False;2;FLOAT;0.75;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CosOpNode;40;-2852.638,1358.395;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;224;-1177.559,155.4525;Float;False;1;0;FLOAT;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;176;-511.7427,-1181.214;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;32;-3711.569,132.2938;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.1;False;4;FLOAT;6.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;102;-2533.546,-1615.076;Float;False;Property;_MidWaterColor4;MidWaterColor4;28;0;Create;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCCompareWithRange;96;-1340.773,-926.9778;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.25;False;2;FLOAT;0.5;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;235;-678.9639,-780.0317;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.05,0.01;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;168;-1409.487,-97.21449;Float;False;3;0;FLOAT;0,0,0,0;False;1;FLOAT;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;90;-1679.581,-1154.566;Float;False;Property;_ShallowColor3;ShallowColor3;23;0;Create;0.3368836,0.6120428,0.6838235,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;220;-1262.76,81.50181;Float;False;Property;_WaveDeepIntensity;WaveDeepIntensity;40;0;Create;1.374183;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;53;-2741.438,1613.59;Float;False;Constant;_Vector1;Vector 1;7;0;Create;-0.5,-0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TFHCCompareWithRange;108;-2801.406,-1721.381;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.75;False;2;FLOAT;1.0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;85;-2837.8,-461.6492;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;111;-3594.929,-1190.474;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.75;False;2;FLOAT;1.0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-2636.189,1239.485;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0005;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;98;-2106.372,-1288.25;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.5;False;2;FLOAT;0.75;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-3245.1,-205.8609;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;5.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;50;-2707.125,1047.856;Float;False;Constant;_Vector0;Vector 0;7;0;Create;0.25,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-2657.189,1355.484;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0005;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;198;-1104.754,-79.78745;Float;False;1;0;FLOAT;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;-2367.189,1159.485;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT2;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-2252.169,975.4083;Float;False;Property;_NormalTiling1;NormalTiling1;5;0;Create;0;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;43;-2835.057,-184.1453;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;60;-2381.848,1470.039;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT2;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;232;-422.9521,-808.2436;Float;True;Property;_SeaFoamMask;SeaFoamMask;34;0;Create;Assets/T_SeaFoam.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;81;-2565.93,-747.1599;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;144;-1063.63,-580.0731;Float;False;Property;_SeaFoamIntensity;SeaFoamIntensity;36;0;Create;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;181;-375.7108,-1185.957;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.002;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;87;-1678.071,-1353.674;Float;False;Property;_ShallowColor4;ShallowColor4;27;0;Create;0.3368836,0.6120428,0.6838235,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCCompareWithRange;95;-1173.805,-1069.95;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.5;False;2;FLOAT;0.75;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-2384.558,1305.376;Float;False;Property;_WaveSpeed;WaveSpeed;7;0;Create;0.1;0;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;175;-551.7427,-1340.214;Float;False;Property;_FoamSpeedVector;FoamSpeedVector;37;0;Create;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-3345.271,84.26419;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;5.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;124;-2248.833,1579.13;Float;False;Property;_NormalTiling2;NormalTiling2;6;0;Create;0;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;219;-939.0773,66.08834;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;99;-1879.7,-1529.51;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.75;False;2;FLOAT;1.0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;39;-1886.558,1313.552;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2.3,2.3;False;1;FLOAT2;1.37,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;223;-707.0089,24.43817;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;196;-730.2359,-568.5273;Float;True;2;2;0;FLOAT;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;237;-76.03882,-691.3134;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;136;-802.0782,-1535.87;Float;False;Property;_FoamTiling;FoamTiling;35;0;Create;0;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;178;-232.5388,-1269.989;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;188;-335.3425,-1070.349;Float;False;Property;_FoamSpeed;FoamSpeed;38;0;Create;0.5;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;94;-987.6968,-1179.914;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.75;False;2;FLOAT;1.0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;41;-1704.392,-513.5506;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-2017.657,1153.169;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0.0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-1880.046,1034.399;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;30;-2993.54,110.4659;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-2005.656,1451.668;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0.0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;13;-1587.242,1173.183;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0.05;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;135;-429.3586,-1569.468;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;18;-1536.467,1772.243;Float;False;Property;_NormalStrenght;NormalStrenght;4;0;Create;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;129;-1433.718,544.4443;Float;False;Property;_HeightColorStrength;HeightColorStrength;32;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;222;-531.8255,20.99439;Float;False;3;0;FLOAT;1.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;187;-44.34253,-1246.349;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;233;-367.8573,-575.3892;Float;True;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;11;-1066.353,-227.3959;Float;False;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;16;-1571.926,1387.327;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0.1;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;163;-1450.645,397.3338;Float;False;3;0;FLOAT;0,0,0,0;False;1;FLOAT;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-1082.642,1493.048;Float;True;Property;_Normal02;Normal02;3;0;Create;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;212;-1462.38,531.0892;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;131;-1198.321,350.5089;Float;False;2;2;0;FLOAT;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;211;-448.3913,1239.697;Float;False;Constant;_Test;Test;39;0;Create;-0.5;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;172;-25.68164,-1455.726;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;221;-544.4675,-208.0668;Float;False;2;2;0;FLOAT;0,0,0,0;False;1;COLOR;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;234;-102.62,-532.8995;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-1085.242,1210.949;Float;True;Property;_Normal01;Normal01;2;0;Create;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;17;-729.2349,1260.315;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;210;60.41437,892.5276;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;207;0.4248629,1287.456;Float;False;Property;_HeightCoeff;HeightCoeff;31;0;Create;1;0;1000;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;205;61.27668,1017.066;Float;False;Constant;_Vector2;Vector 2;38;0;Create;0,0,1;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;189;100.0865,-1146.896;Float;True;Property;_T_SeaFoam;T_SeaFoam;33;0;Create;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;200;125.3124,-560.2127;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;154;-967.6512,975.8253;Float;True;Property;_InputNormal;InputNormal;0;0;Create;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;-244.7889,-213.1226;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;133;203.5227,-137.0332;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;123;247.3401,188.6354;Float;False;Property;_Opacity;Opacity;9;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;146;-501.832,1009.956;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;206;377.5349,999.7948;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT3;0;False;2;FLOAT;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;66;187.8937,69.40292;Float;False;Property;_Smoothness;Smoothness;8;0;Create;2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;546.1237,-12.8716;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Water/WaterShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Translucent;0.5;True;False;0;False;Opaque;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;0;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;73;0;74;0
WireConnection;227;0;147;1
WireConnection;83;0;82;0
WireConnection;166;0;147;1
WireConnection;7;0;73;0
WireConnection;226;0;228;0
WireConnection;226;1;227;0
WireConnection;119;0;46;0
WireConnection;119;1;82;0
WireConnection;119;2;31;0
WireConnection;104;0;89;0
WireConnection;104;3;103;0
WireConnection;104;4;80;0
WireConnection;109;0;89;0
WireConnection;109;3;112;0
WireConnection;109;4;9;0
WireConnection;84;0;7;0
WireConnection;84;1;83;0
WireConnection;97;0;89;0
WireConnection;97;3;100;0
WireConnection;97;4;42;0
WireConnection;107;0;89;0
WireConnection;107;3;105;0
WireConnection;107;4;104;0
WireConnection;190;0;166;0
WireConnection;190;1;191;0
WireConnection;45;0;119;0
WireConnection;225;0;226;0
WireConnection;225;1;164;0
WireConnection;225;2;165;0
WireConnection;118;0;31;0
WireConnection;118;1;46;0
WireConnection;33;0;35;2
WireConnection;110;0;89;0
WireConnection;110;3;113;0
WireConnection;110;4;109;0
WireConnection;40;0;35;2
WireConnection;224;0;225;0
WireConnection;176;0;180;2
WireConnection;32;0;118;0
WireConnection;96;0;89;0
WireConnection;96;3;91;0
WireConnection;96;4;10;0
WireConnection;235;0;236;0
WireConnection;168;0;190;0
WireConnection;168;1;164;0
WireConnection;168;2;165;0
WireConnection;108;0;89;0
WireConnection;108;3;106;0
WireConnection;108;4;107;0
WireConnection;85;0;84;0
WireConnection;111;0;89;0
WireConnection;111;3;114;0
WireConnection;111;4;110;0
WireConnection;56;0;33;0
WireConnection;98;0;89;0
WireConnection;98;3;101;0
WireConnection;98;4;97;0
WireConnection;44;0;7;0
WireConnection;44;1;45;0
WireConnection;62;0;40;0
WireConnection;198;0;168;0
WireConnection;58;0;56;0
WireConnection;58;1;50;0
WireConnection;43;0;44;0
WireConnection;60;0;62;0
WireConnection;60;1;53;0
WireConnection;232;1;235;0
WireConnection;81;0;108;0
WireConnection;81;1;111;0
WireConnection;81;2;85;0
WireConnection;181;0;176;0
WireConnection;95;0;89;0
WireConnection;95;3;90;0
WireConnection;95;4;96;0
WireConnection;27;0;7;0
WireConnection;27;1;32;0
WireConnection;219;0;224;0
WireConnection;219;1;220;0
WireConnection;99;0;89;0
WireConnection;99;3;102;0
WireConnection;99;4;98;0
WireConnection;39;0;124;0
WireConnection;223;0;219;0
WireConnection;196;0;198;0
WireConnection;196;1;144;0
WireConnection;237;0;232;1
WireConnection;178;0;175;0
WireConnection;178;1;181;0
WireConnection;94;0;89;0
WireConnection;94;3;87;0
WireConnection;94;4;95;0
WireConnection;41;0;99;0
WireConnection;41;1;81;0
WireConnection;41;2;43;0
WireConnection;59;0;58;0
WireConnection;59;1;65;0
WireConnection;12;0;79;0
WireConnection;30;0;27;0
WireConnection;61;0;60;0
WireConnection;61;1;65;0
WireConnection;13;0;12;0
WireConnection;13;2;59;0
WireConnection;135;0;136;0
WireConnection;222;2;223;0
WireConnection;187;0;178;0
WireConnection;187;1;188;0
WireConnection;233;0;196;0
WireConnection;233;1;237;0
WireConnection;11;0;94;0
WireConnection;11;1;41;0
WireConnection;11;2;30;0
WireConnection;16;0;39;0
WireConnection;16;2;61;0
WireConnection;163;0;147;1
WireConnection;163;1;164;0
WireConnection;163;2;165;0
WireConnection;3;1;16;0
WireConnection;3;5;18;0
WireConnection;212;0;147;1
WireConnection;131;0;163;0
WireConnection;131;1;129;0
WireConnection;172;0;135;0
WireConnection;172;2;187;0
WireConnection;221;0;222;0
WireConnection;221;1;11;0
WireConnection;234;0;233;0
WireConnection;1;1;13;0
WireConnection;1;5;18;0
WireConnection;17;0;1;0
WireConnection;17;1;3;0
WireConnection;210;0;212;0
WireConnection;210;1;211;0
WireConnection;189;1;172;0
WireConnection;200;0;234;0
WireConnection;127;0;221;0
WireConnection;127;1;131;0
WireConnection;133;0;189;0
WireConnection;133;1;127;0
WireConnection;133;2;200;0
WireConnection;146;0;154;0
WireConnection;146;1;17;0
WireConnection;206;0;210;0
WireConnection;206;1;205;0
WireConnection;206;2;207;0
WireConnection;0;0;133;0
WireConnection;0;1;146;0
WireConnection;0;4;66;0
WireConnection;0;9;123;0
WireConnection;0;11;206;0
ASEEND*/
//CHKSM=544CB3C9773C20266D3EEA42EFEFE0C399B908FC