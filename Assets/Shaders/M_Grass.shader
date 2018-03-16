// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "grass"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_distance("distance", Float) = 0
		_Strength("Strength", Float) = 0
		_windDirection("windDirection", Vector) = (0,0,0,0)
		_Amplitude("Amplitude", Float) = 0
		_T_Grass("T_Grass", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "Transparent+0" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _T_Grass;
		uniform float4 _T_Grass_ST;
		uniform float3 _humanPos;
		uniform float _distance;
		uniform float _Strength;
		uniform float3 _windDirection;
		uniform float _Amplitude;
		uniform float _Cutoff = 0.5;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float3 temp_output_12_0 = (_humanPos).xyz;
			float clampResult19 = clamp( ( distance( ase_worldPos , temp_output_12_0 ) / _distance ) , 0.0 , 1.0 );
			float2 uv_TexCoord23 = v.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
			float temp_output_25_0 = (uv_TexCoord23).y;
			float temp_output_27_0 = ( temp_output_25_0 * temp_output_25_0 );
			float3 normalizeResult21 = normalize( ( ase_worldPos - temp_output_12_0 ) );
			float2 appendResult33 = (float2(_Strength , _Strength));
			float3 appendResult34 = (float3(appendResult33 , 0.0));
			float simplePerlin2D51 = snoise( float2( 1,1 ) );
			float3 ase_worldNormal = UnityObjectToWorldNormal( v.normal );
			float3 normalizeResult40 = normalize( ( ( _windDirection * simplePerlin2D51 ) + ase_worldNormal ) );
			float mulTime43 = _Time.y * 0.5;
			v.vertex.xyz += ( ( ( ( 1.0 - clampResult19 ) * temp_output_27_0 ) * ( normalizeResult21 * appendResult34 ) ) + ( temp_output_27_0 * ( normalizeResult40 * sin( ( UNITY_PI + mulTime43 ) ) * _Amplitude ) ) );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_T_Grass = i.uv_texcoord * _T_Grass_ST.xy + _T_Grass_ST.zw;
			float4 tex2DNode66 = tex2D( _T_Grass, uv_T_Grass );
			o.Albedo = tex2DNode66.rgb;
			o.Alpha = 1;
			clip( tex2DNode66.a - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=14101
1927;29;1586;824;530.9493;1217.26;2.13763;True;True
Node;AmplifyShaderEditor.CommentaryNode;63;-1469.262,53.8733;Float;False;250;234;dynamicGrass ????;1;57;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector3Node;57;-1419.262,103.8733;Float;False;Global;_humanPos;_humanPos;5;0;Create;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;22;-831.1523,-319.4599;Float;False;732.7385;267;distance;5;13;17;20;19;14;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;48;-856.0604,718.5074;Float;False;1958.241;584.9286;WindDirection;13;41;40;47;45;39;52;46;38;43;36;55;51;65;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ComponentMaskNode;12;-1188,123;Float;False;True;True;True;False;1;0;FLOAT3;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;15;-1205,-72;Float;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.CommentaryNode;35;-744.0416,326.586;Float;False;708.0002;290.3311;Strength;6;16;21;30;31;33;34;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector3Node;36;-808.2427,772.1432;Float;False;Property;_windDirection;windDirection;3;0;Create;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NoiseGeneratorNode;51;-795.7573,944.1705;Float;False;Simplex2D;1;0;FLOAT2;1,1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-772.1523,-167.4598;Float;False;Property;_distance;distance;1;0;Create;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;14;-781.1523,-269.4598;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;62;-723.3975,53.35147;Float;False;753.5518;218.5366;mask;3;23;25;27;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;43;-328.027,1198.418;Float;False;1;0;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-486.2516,807.5551;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;23;-673.3975,115.8881;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PiNode;46;-336.8662,1127.704;Float;False;1;0;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;13;-582.1523,-267.4598;Float;False;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-694.0417,488.9171;Float;False;Property;_Strength;Strength;2;0;Create;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;38;-345.6246,942.0355;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleSubtractOpNode;16;-657.3372,378.3206;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0.0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;19;-432.6046,-265.0075;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;39;-44.57849,812.7634;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0.0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;33;-541.0416,483.9171;Float;False;FLOAT2;4;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;25;-414.9449,106.8227;Float;False;False;True;True;True;1;0;FLOAT2;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;45;-129.1465,1131.019;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-138.8458,103.3515;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;34;-394.0417,482.9171;Float;False;FLOAT3;4;0;FLOAT2;0,0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;3;FLOAT;0.0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalizeNode;40;147.0767,822.7538;Float;False;1;0;FLOAT3;0.0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SinOpNode;47;108.4056,1095.662;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;21;-409.1259,376.5861;Float;False;1;0;FLOAT3;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;20;-285.4138,-267.1029;Float;False;1;0;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;282.5815,1107.537;Float;False;Property;_Amplitude;Amplitude;5;0;Create;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;210.3207,43.17303;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;370.982,823.756;Float;True;3;3;0;FLOAT3;0,0,0;False;1;FLOAT;0.0,0,0;False;2;FLOAT;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-205.0416,378.9171;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;833.8564,789.9713;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT3;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;443.1661,218.676;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT3;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;66;1060.235,-725.997;Float;True;Property;_T_Grass;T_Grass;6;0;Create;Assets/Art/Textures/Grass/T_Grass.png;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;50;1202.288,224.4275;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0.0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;64;627.2184,-726.5353;Float;True;Property;_T_Fern;T_Fern;4;0;Create;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1449.306,-297.2644;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;grass;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;0;False;0;0;Custom;0.5;True;True;0;True;TransparentCutout;Transparent;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;One;One;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;0;0;False;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;12;0;57;0
WireConnection;14;0;15;0
WireConnection;14;1;12;0
WireConnection;52;0;36;0
WireConnection;52;1;51;0
WireConnection;13;0;14;0
WireConnection;13;1;17;0
WireConnection;16;0;15;0
WireConnection;16;1;12;0
WireConnection;19;0;13;0
WireConnection;39;0;52;0
WireConnection;39;1;38;0
WireConnection;33;0;31;0
WireConnection;33;1;31;0
WireConnection;25;0;23;0
WireConnection;45;0;46;0
WireConnection;45;1;43;0
WireConnection;27;0;25;0
WireConnection;27;1;25;0
WireConnection;34;0;33;0
WireConnection;40;0;39;0
WireConnection;47;0;45;0
WireConnection;21;0;16;0
WireConnection;20;0;19;0
WireConnection;28;0;20;0
WireConnection;28;1;27;0
WireConnection;41;0;40;0
WireConnection;41;1;47;0
WireConnection;41;2;65;0
WireConnection;30;0;21;0
WireConnection;30;1;34;0
WireConnection;55;0;27;0
WireConnection;55;1;41;0
WireConnection;29;0;28;0
WireConnection;29;1;30;0
WireConnection;50;0;29;0
WireConnection;50;1;55;0
WireConnection;0;0;66;0
WireConnection;0;10;66;4
WireConnection;0;11;50;0
ASEEND*/
//CHKSM=08FB35A9E129BC3A0118E9FD90507AFA47304EA8