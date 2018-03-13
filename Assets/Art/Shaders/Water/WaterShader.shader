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
		_HeightColorStrength("HeightColorStrength", Range( 0 , 1)) = 0
		_T_SeaFoam("T_SeaFoam", 2D) = "white" {}
		_FoamTiling("FoamTiling", Range( 0 , 20)) = 0
		_SeaFoamIntensity("SeaFoamIntensity", Range( 0 , 5)) = 0
		_FoamSpeedVector("FoamSpeedVector", Vector) = (0,0,0,0)
		_FoamSpeed("FoamSpeed", Range( 0 , 0.5)) = 0.5
		_FoamDistance("FoamDistance", Range( 0 , 5)) = 0
		_HeightCoeff("HeightCoeff", Range( 0 , 1000)) = 1
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
		uniform sampler2D _InputHeight;
		uniform float4 _InputHeight_ST;
		uniform float _FoamDistance;
		uniform float _SeaFoamIntensity;
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
		uniform float _Smoothness;
		uniform float _Opacity;
		uniform float _HeightCoeff;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float2 uv_InputHeight = v.texcoord * _InputHeight_ST.xy + _InputHeight_ST.zw;
			float4 tex2DNode147 = tex2Dlod( _InputHeight, float4( uv_InputHeight, 0, 0.0) );
			v.vertex.xyz += ( ( tex2DNode147.r + -0.5 ) * float3(0,1,0) * _HeightCoeff );
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
			float4 temp_cast_3 = (0.0).xxxx;
			float4 temp_cast_4 = (1.0).xxxx;
			float4 clampResult168 = clamp( ( ( 1.0 - tex2DNode147 ) * _FoamDistance ) , temp_cast_3 , temp_cast_4 );
			float4 lerpResult171 = lerp( tex2D( _T_SeaFoam, panner172 ) , float4( 0,0,0,0 ) , ( 1.0 - Luminance(( ( 1.0 - clampResult168 ) * _SeaFoamIntensity ).rgb) ));
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
			float4 temp_cast_6 = (0.0).xxxx;
			float4 temp_cast_7 = (1.0).xxxx;
			float4 clampResult163 = clamp( ( tex2DNode147 * 3 ) , temp_cast_6 , temp_cast_7 );
			float4 temp_output_131_0 = ( clampResult163 + _HeightColorStrength );
			float4 temp_cast_8 = (0.5).xxxx;
			float4 temp_cast_9 = (1.0).xxxx;
			float4 temp_cast_10 = (0.0).xxxx;
			float4 temp_cast_11 = (0.8).xxxx;
			float4 temp_cast_12 = (0.0).xxxx;
			float4 temp_cast_13 = (1.0).xxxx;
			float4 clampResult160 = clamp( (temp_cast_10 + (temp_output_131_0 - temp_cast_8) * (temp_cast_11 - temp_cast_10) / (temp_cast_9 - temp_cast_8)) , temp_cast_12 , temp_cast_13 );
			float4 lerpResult133 = lerp( lerpResult171 , ( lerpResult11 * temp_output_131_0 ) , clampResult160.r);
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
472;29;1066;1004;1752.669;-229.8858;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;74;-4881.021,-210.8501;Float;False;Property;_OverallDepth;OverallDepth;13;0;Create;0;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;147;-2208.641,116.51;Float;True;Property;_InputHeight;InputHeight;1;0;Create;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;112;-4391.618,-1299.422;Float;False;Property;_FarDeepColor2;FarDeepColor2;22;0;Create;0.06060771,0.4338235,0.155842,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;31;-4353.957,121.813;Float;False;Property;_ShallowWaterDistance;ShallowWaterDistance;10;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;103;-3436.88,-1538.95;Float;False;Property;_DeepColor2;DeepColor2;21;0;Create;0.06060771,0.4338235,0.155842,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;166;-1881.756,-127.5912;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;191;-1994.904,-35.9842;Float;False;Property;_FoamDistance;FoamDistance;37;0;Create;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;9;-4394.071,-1047.149;Float;False;Property;_FarDeepColor1;FarDeepColor1;18;0;Create;0.466317,0.4689327,0.8455882,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;46;-4263.246,-405.0437;Float;False;Property;_MidWaterDistance;MidWaterDistance;12;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-4145.446,-739.2268;Float;False;Property;_FarDeepDistance;FarDeepDistance;11;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TimeNode;35;-3177.961,869.5292;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TimeNode;180;-760.5388,-1202.989;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;73;-4578.022,-204.8501;Float;False;True;1;0;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;80;-3455.196,-1332.132;Float;False;Property;_DeepColor1;DeepColor1;17;0;Create;0.466317,0.4689327,0.8455882,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;89;-3843.225,-2668.656;Float;False;Property;_ColorChange;ColorChange;14;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;109;-4106.268,-1165.175;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.25;False;2;FLOAT;0.5;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;42;-2555.166,-1047.212;Float;False;Property;_MidWaterColor1;MidWaterColor1;16;0;Create;0.466317,0.4689327,0.8455882,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;83;-3561.086,-690.6689;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.0;False;4;FLOAT;6.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;7;-3682.777,-206.6383;Float;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;104;-3170.558,-1452.974;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.25;False;2;FLOAT;0.5;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SinOpNode;33;-2853.557,833.881;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;176;-511.7427,-1181.214;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;164;-1636.572,195.593;Float;False;Constant;_Float4;Float 4;35;0;Create;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;40;-2852.638,962.9774;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;100;-2550.216,-1231.939;Float;False;Property;_MidWaterColor2;MidWaterColor2;20;0;Create;0.06060771,0.4338235,0.155842,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;119;-3797.57,-413.92;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;113;-4379.821,-1506.106;Float;False;Property;_FarDeepColor3;FarDeepColor3;26;0;Create;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;105;-3424.649,-1731.899;Float;False;Property;_DeepColor3;DeepColor3;25;0;Create;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;165;-1635.272,265.7931;Float;False;Constant;_Float5;Float 5;35;0;Create;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;190;-1637.957,-132.2351;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;114;-4374.407,-1705.821;Float;False;Property;_FarDeepColor4;FarDeepColor4;30;0;Create;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCCompareWithRange;110;-3870.45,-1169.22;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.5;False;2;FLOAT;0.75;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;53;-2741.438,1218.173;Float;False;Constant;_Vector1;Vector 1;7;0;Create;-0.5,-0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TFHCCompareWithRange;97;-2283.893,-1145.964;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.25;False;2;FLOAT;0.5;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;10;-1689.572,-739.6265;Float;False;Property;_ShallowColor1;ShallowColor1;15;0;Create;0.3368836,0.6120428,0.6838235,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-2636.189,844.0674;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0005;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;91;-1673.947,-928.0525;Float;False;Property;_ShallowColor2;ShallowColor2;19;0;Create;0.3368836,0.6120428,0.6838235,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;181;-375.7108,-1185.957;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.002;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-2657.189,960.067;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0005;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;107;-2993.037,-1595.262;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.5;False;2;FLOAT;0.75;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;45;-3559.342,-413.1642;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.1;False;4;FLOAT;6.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;50;-2707.125,652.4383;Float;False;Constant;_Vector0;Vector 0;7;0;Create;0.25,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-3246.843,-483.3651;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;5.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;175;-551.7427,-1340.214;Float;False;Property;_FoamSpeedVector;FoamSpeedVector;35;0;Create;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ClampOpNode;118;-3929.917,124.2814;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;101;-2537.984,-1424.887;Float;False;Property;_MidWaterColor3;MidWaterColor3;24;0;Create;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;168;-1409.487,-130.0757;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;106;-3410.107,-2001.831;Float;False;Property;_DeepColor4;DeepColor4;29;0;Create;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;198;-1104.754,-79.78745;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-2384.558,909.9583;Float;False;Property;_WaveSpeed;WaveSpeed;7;0;Create;0.1;0;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;60;-2381.848,1074.622;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT2;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;102;-2533.546,-1615.076;Float;False;Property;_MidWaterColor4;MidWaterColor4;28;0;Create;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ScaleNode;148;-1810.157,80.6828;Float;True;3;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;111;-3594.929,-1190.474;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.75;False;2;FLOAT;1.0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;85;-2837.8,-461.6492;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;98;-2106.372,-1288.25;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.5;False;2;FLOAT;0.75;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-2252.169,579.9909;Float;False;Property;_NormalTiling1;NormalTiling1;5;0;Create;0;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;188;-335.3425,-1070.349;Float;False;Property;_FoamSpeed;FoamSpeed;36;0;Create;0.5;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;136;-802.0782,-1535.87;Float;False;Property;_FoamTiling;FoamTiling;33;0;Create;0;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;-2367.189,764.0674;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT2;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;178;-232.5388,-1269.989;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;90;-1679.581,-1154.566;Float;False;Property;_ShallowColor3;ShallowColor3;23;0;Create;0.3368836,0.6120428,0.6838235,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-3245.1,-205.8609;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;5.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;108;-2801.406,-1721.381;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.75;False;2;FLOAT;1.0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;144;-1063.63,-580.0731;Float;False;Property;_SeaFoamIntensity;SeaFoamIntensity;34;0;Create;0;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;124;-2248.833,1183.713;Float;False;Property;_NormalTiling2;NormalTiling2;6;0;Create;0;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;32;-3711.569,132.2938;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.1;False;4;FLOAT;6.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;96;-1340.773,-926.9778;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.25;False;2;FLOAT;0.5;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;87;-1678.071,-1353.674;Float;False;Property;_ShallowColor4;ShallowColor4;27;0;Create;0.3368836,0.6120428,0.6838235,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-3345.271,84.26419;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;5.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-1880.046,638.9815;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;135;-429.3586,-1569.468;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;81;-2565.93,-747.1599;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;99;-1879.7,-1529.51;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.75;False;2;FLOAT;1.0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-2017.657,757.7512;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0.0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;95;-1173.805,-1069.95;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.5;False;2;FLOAT;0.75;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;129;-1416.445,250.8033;Float;False;Property;_HeightColorStrength;HeightColorStrength;31;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;43;-2835.057,-184.1453;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;163;-1433.372,103.6928;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;196;-730.2359,-568.5273;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;187;-44.34253,-1246.349;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-2005.656,1056.251;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0.0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;39;-1886.558,918.1344;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2.3,2.3;False;1;FLOAT2;1.37,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;172;-25.68164,-1455.726;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;156;-703.2482,206.4671;Float;False;Constant;_Float0;Float 0;35;0;Create;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;30;-2993.54,110.4659;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;158;-695.1324,357.0118;Float;False;Constant;_Float2;Float 2;35;0;Create;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;159;-692.1324,441.0118;Float;False;Constant;_Float3;Float 3;35;0;Create;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;13;-1587.242,777.7659;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0.05;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;131;-1181.048,56.86792;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;41;-1704.392,-513.5506;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;157;-692.1324,279.0119;Float;False;Constant;_Float1;Float 1;35;0;Create;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCCompareWithRange;94;-987.6968,-1179.914;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.75;False;2;FLOAT;1.0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCGrayscale;199;-453.8683,-572.5667;Float;False;0;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;18;-1536.467,1376.826;Float;False;Property;_NormalStrenght;NormalStrenght;4;0;Create;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;16;-1571.926,991.9102;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0.1;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;3;-1082.642,1097.631;Float;True;Property;_Normal02;Normal02;3;0;Create;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;155;-477.7229,62.78345;Float;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;211;-1935.166,423.111;Float;False;Constant;_Test;Test;39;0;Create;-0.5;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;189;100.0865,-1146.896;Float;True;Property;_T_SeaFoam;T_SeaFoam;32;0;Create;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;11;-1066.353,-227.3959;Float;False;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-1085.242,815.5317;Float;True;Property;_Normal01;Normal01;2;0;Create;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;200;-215.8911,-575.6633;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;210;-1378.704,381.8927;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;160;-272.1228,287.2042;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;207;-1335.167,620.6909;Float;False;Property;_HeightCoeff;HeightCoeff;38;0;Create;1;0;1000;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;205;-1647.141,528.1935;Float;False;Constant;_Vector2;Vector 2;38;0;Create;0,1,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.LerpOp;171;-18.22168,-628.5142;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;154;-967.6512,580.4079;Float;True;Property;_InputNormal;InputNormal;0;0;Create;None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;17;-729.2349,864.8979;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;-387.7889,-200.1226;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;123;247.3401,188.6354;Float;False;Property;_Opacity;Opacity;9;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.BlendNormalsNode;146;-501.832,614.5388;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;66;187.8937,69.40292;Float;False;Property;_Smoothness;Smoothness;8;0;Create;2;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;133;203.5227,-137.0332;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;206;-985.5442,417.5145;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT3;0;False;2;FLOAT;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;546.1237,-12.8716;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Water/WaterShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Translucent;0.5;True;False;0;False;Opaque;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;0;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;166;0;147;0
WireConnection;73;0;74;0
WireConnection;109;0;89;0
WireConnection;109;3;112;0
WireConnection;109;4;9;0
WireConnection;83;0;82;0
WireConnection;7;0;73;0
WireConnection;104;0;89;0
WireConnection;104;3;103;0
WireConnection;104;4;80;0
WireConnection;33;0;35;2
WireConnection;176;0;180;2
WireConnection;40;0;35;2
WireConnection;119;0;46;0
WireConnection;119;1;82;0
WireConnection;119;2;31;0
WireConnection;190;0;166;0
WireConnection;190;1;191;0
WireConnection;110;0;89;0
WireConnection;110;3;113;0
WireConnection;110;4;109;0
WireConnection;97;0;89;0
WireConnection;97;3;100;0
WireConnection;97;4;42;0
WireConnection;56;0;33;0
WireConnection;181;0;176;0
WireConnection;62;0;40;0
WireConnection;107;0;89;0
WireConnection;107;3;105;0
WireConnection;107;4;104;0
WireConnection;45;0;119;0
WireConnection;84;0;7;0
WireConnection;84;1;83;0
WireConnection;118;0;31;0
WireConnection;118;1;46;0
WireConnection;168;0;190;0
WireConnection;168;1;164;0
WireConnection;168;2;165;0
WireConnection;198;0;168;0
WireConnection;60;0;62;0
WireConnection;60;1;53;0
WireConnection;148;0;147;0
WireConnection;111;0;89;0
WireConnection;111;3;114;0
WireConnection;111;4;110;0
WireConnection;85;0;84;0
WireConnection;98;0;89;0
WireConnection;98;3;101;0
WireConnection;98;4;97;0
WireConnection;58;0;56;0
WireConnection;58;1;50;0
WireConnection;178;0;175;0
WireConnection;178;1;181;0
WireConnection;44;0;7;0
WireConnection;44;1;45;0
WireConnection;108;0;89;0
WireConnection;108;3;106;0
WireConnection;108;4;107;0
WireConnection;32;0;118;0
WireConnection;96;0;89;0
WireConnection;96;3;91;0
WireConnection;96;4;10;0
WireConnection;27;0;7;0
WireConnection;27;1;32;0
WireConnection;12;0;79;0
WireConnection;135;0;136;0
WireConnection;81;0;108;0
WireConnection;81;1;111;0
WireConnection;81;2;85;0
WireConnection;99;0;89;0
WireConnection;99;3;102;0
WireConnection;99;4;98;0
WireConnection;59;0;58;0
WireConnection;59;1;65;0
WireConnection;95;0;89;0
WireConnection;95;3;90;0
WireConnection;95;4;96;0
WireConnection;43;0;44;0
WireConnection;163;0;148;0
WireConnection;163;1;164;0
WireConnection;163;2;165;0
WireConnection;196;0;198;0
WireConnection;196;1;144;0
WireConnection;187;0;178;0
WireConnection;187;1;188;0
WireConnection;61;0;60;0
WireConnection;61;1;65;0
WireConnection;39;0;124;0
WireConnection;172;0;135;0
WireConnection;172;2;187;0
WireConnection;30;0;27;0
WireConnection;13;0;12;0
WireConnection;13;2;59;0
WireConnection;131;0;163;0
WireConnection;131;1;129;0
WireConnection;41;0;99;0
WireConnection;41;1;81;0
WireConnection;41;2;43;0
WireConnection;94;0;89;0
WireConnection;94;3;87;0
WireConnection;94;4;95;0
WireConnection;199;0;196;0
WireConnection;16;0;39;0
WireConnection;16;2;61;0
WireConnection;3;1;16;0
WireConnection;3;5;18;0
WireConnection;155;0;131;0
WireConnection;155;1;156;0
WireConnection;155;2;157;0
WireConnection;155;3;158;0
WireConnection;155;4;159;0
WireConnection;189;1;172;0
WireConnection;11;0;94;0
WireConnection;11;1;41;0
WireConnection;11;2;30;0
WireConnection;1;1;13;0
WireConnection;1;5;18;0
WireConnection;200;0;199;0
WireConnection;210;0;147;1
WireConnection;210;1;211;0
WireConnection;160;0;155;0
WireConnection;160;1;158;0
WireConnection;160;2;157;0
WireConnection;171;0;189;0
WireConnection;171;2;200;0
WireConnection;17;0;1;0
WireConnection;17;1;3;0
WireConnection;127;0;11;0
WireConnection;127;1;131;0
WireConnection;146;0;154;0
WireConnection;146;1;17;0
WireConnection;133;0;171;0
WireConnection;133;1;127;0
WireConnection;133;2;160;0
WireConnection;206;0;210;0
WireConnection;206;1;205;0
WireConnection;206;2;207;0
WireConnection;0;0;133;0
WireConnection;0;1;146;0
WireConnection;0;4;66;0
WireConnection;0;9;123;0
WireConnection;0;11;206;0
ASEEND*/
//CHKSM=7024E10BDAC88A69A1C579A6EF75219DF813A998