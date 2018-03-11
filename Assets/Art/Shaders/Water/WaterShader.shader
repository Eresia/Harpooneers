// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Water/WaterShader"
{
	Properties
	{
		_Normal01("Normal01", 2D) = "bump" {}
		_Normal02("Normal02", 2D) = "bump" {}
		_ShallowColor("ShallowColor", Color) = (0.3368836,0.6120428,0.6838235,0)
		_MidWaterColor("MidWaterColor", Color) = (0.466317,0.4689327,0.8455882,0)
		_NormalStrenght("NormalStrenght", Range( 0 , 1)) = 0.5
		_DeepColor("DeepColor", Color) = (0.466317,0.4689327,0.8455882,0)
		_MidWaterDistance("MidWaterDistance", Range( 0 , 1)) = 0
		_ShallowWaterDistance("ShallowWaterDistance", Range( 0 , 1)) = 0
		_WaveSpeed("WaveSpeed", Range( 0 , 1.5)) = 0.1
		_OverallDepth("OverallDepth", Range( 0 , 3)) = 0
		_NormalTiling("NormalTiling", Range( 0 , 10)) = 0
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
		#pragma surface surf Standard keepalpha noshadow exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		uniform float _NormalStrenght;
		uniform sampler2D _Normal01;
		uniform float _WaveSpeed;
		uniform float _NormalTiling;
		uniform sampler2D _Normal02;
		uniform float4 _ShallowColor;
		uniform float4 _MidWaterColor;
		uniform float4 _DeepColor;
		uniform sampler2D _CameraDepthTexture;
		uniform float _OverallDepth;
		uniform float _MidWaterDistance;
		uniform float _ShallowWaterDistance;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
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
			float clampResult43 = clamp( ( temp_output_7_0 * (0.1 + (_MidWaterDistance - 0.0) * (6.0 - 0.1) / (1.0 - 0.0)) ) , 0.0 , 1.0 );
			float4 lerpResult41 = lerp( _MidWaterColor , _DeepColor , clampResult43);
			float clampResult30 = clamp( ( temp_output_7_0 * (0.1 + (_ShallowWaterDistance - 0.0) * (6.0 - 0.1) / (1.0 - 0.0)) ) , 0.0 , 1.0 );
			float4 lerpResult11 = lerp( _ShallowColor , lerpResult41 , clampResult30);
			o.Albedo = lerpResult11.rgb;
			o.Smoothness = 1.0;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14101
6;33;1808;1004;5132.202;945.6899;2.161233;True;False
Node;AmplifyShaderEditor.TimeNode;35;-3160.134,848.73;Float;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;74;-3504.099,-235.7029;Float;False;Property;_OverallDepth;OverallDepth;10;0;Create;0;0;3;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;33;-2853.557,833.881;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CosOpNode;40;-2840.782,998.547;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;53;-2741.438,1218.173;Float;False;Constant;_Vector1;Vector 1;7;0;Create;-0.5,-0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DepthFade;73;-3201.099,-229.7029;Float;False;True;1;0;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-2657.189,960.067;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0005;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-2806.333,-428.2344;Float;False;Property;_MidWaterDistance;MidWaterDistance;6;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;56;-2636.189,844.0674;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0005;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;50;-2707.125,652.4383;Float;False;Constant;_Vector0;Vector 0;7;0;Create;0.25,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;65;-2288.133,952.8138;Float;False;Property;_WaveSpeed;WaveSpeed;8;0;Create;0.1;0;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;58;-2367.189,764.0674;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT2;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;60;-2381.848,1074.622;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT2;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.AbsOpNode;7;-2305.854,-231.491;Float;True;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;45;-2418.301,-533.374;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.1;False;4;FLOAT;6.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-2565.66,295.5608;Float;False;Property;_ShallowWaterDistance;ShallowWaterDistance;7;0;Create;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-2252.169,579.9909;Float;False;Property;_NormalTiling;NormalTiling;10;0;Create;0;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;32;-2257.273,296.0416;Float;False;5;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;3;FLOAT;0.1;False;4;FLOAT;6.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-1968.157,1056.251;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0.0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-1950.799,-452.7579;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;5.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-1880.046,638.9815;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-2017.657,757.7512;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0.0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;39;-1886.558,918.1344;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;2.3,2.3;False;1;FLOAT2;1.37,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-1968.348,59.41154;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;5.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;16;-1571.926,991.9102;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0.1;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;9;-1718.979,-871.5757;Float;False;Property;_DeepColor;DeepColor;5;0;Create;0.466317,0.4689327,0.8455882,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;42;-1722.132,-1080.583;Float;False;Property;_MidWaterColor;MidWaterColor;3;0;Create;0.466317,0.4689327,0.8455882,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;18;-1536.467,1376.826;Float;False;Property;_NormalStrenght;NormalStrenght;4;0;Create;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;13;-1587.242,777.7659;Float;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0.1,0.05;False;1;FLOAT;1.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;43;-1540.757,-431.0422;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;30;-1616.618,85.61325;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-1723.867,-1311.636;Float;False;Property;_ShallowColor;ShallowColor;2;0;Create;0.3368836,0.6120428,0.6838235,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;41;-1232.726,-582.0836;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;3;-1082.642,1097.631;Float;True;Property;_Normal02;Normal02;1;0;Create;Assets/Normal02.png;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-1085.242,815.5317;Float;True;Property;_Normal01;Normal01;0;0;Create;Assets/Normal01.png;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.BlendNormalsNode;17;-729.2349,864.8979;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-355.6011,82.6297;Float;False;Constant;_Float0;Float 0;9;0;Create;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;11;-865.1147,-376.4427;Float;False;3;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Water/WaterShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;Back;0;0;False;0;0;Translucent;0.5;True;False;0;False;Opaque;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;False;0;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;33;0;35;2
WireConnection;40;0;35;2
WireConnection;73;0;74;0
WireConnection;62;0;40;0
WireConnection;56;0;33;0
WireConnection;58;0;56;0
WireConnection;58;1;50;0
WireConnection;60;0;62;0
WireConnection;60;1;53;0
WireConnection;7;0;73;0
WireConnection;45;0;46;0
WireConnection;32;0;31;0
WireConnection;61;0;60;0
WireConnection;61;1;65;0
WireConnection;44;0;7;0
WireConnection;44;1;45;0
WireConnection;12;0;79;0
WireConnection;59;0;58;0
WireConnection;59;1;65;0
WireConnection;39;0;79;0
WireConnection;27;0;7;0
WireConnection;27;1;32;0
WireConnection;16;0;39;0
WireConnection;16;2;61;0
WireConnection;13;0;12;0
WireConnection;13;2;59;0
WireConnection;43;0;44;0
WireConnection;30;0;27;0
WireConnection;41;0;42;0
WireConnection;41;1;9;0
WireConnection;41;2;43;0
WireConnection;3;1;16;0
WireConnection;3;5;18;0
WireConnection;1;1;13;0
WireConnection;1;5;18;0
WireConnection;17;0;1;0
WireConnection;17;1;3;0
WireConnection;11;0;10;0
WireConnection;11;1;41;0
WireConnection;11;2;30;0
WireConnection;0;0;11;0
WireConnection;0;1;17;0
WireConnection;0;4;66;0
ASEEND*/
//CHKSM=048B14A3BE90E8DEDF3DB3E158500D12EE14848D