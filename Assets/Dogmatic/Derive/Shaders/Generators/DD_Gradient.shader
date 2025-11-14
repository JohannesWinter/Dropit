// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

Shader "Hidden/Derive/Generators/Gradient"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		[Toggle]_LinearAsymmetrical("LinearAsymmetrical", Float) = 0
		[Toggle]_LinearSymmetrical("LinearSymmetrical", Float) = 0
		[Toggle]_LinearSquare("LinearSquare", Float) = 0
		[Toggle]_SmoothDiamond("SmoothDiamond", Float) = 0
		[Toggle]_SinAsymmetrical("SinAsymmetrical", Float) = 0
		[Toggle]_SinSymmetrical("SinSymmetrical", Float) = 0
		[Toggle]_SinSquare("SinSquare", Float) = 0
		[Toggle]_SinSmoothSquare("SinSmoothSquare", Float) = 0
		[Toggle]_LogisticAsymmetrical("LogisticAsymmetrical", Float) = 0
		[Toggle]_LogisticSymmetrical("LogisticSymmetrical", Float) = 0
		[Toggle]_LogisticSquare("LogisticSquare", Float) = 0
		[Toggle]_LogisticSmoothSquare("LogisticSmoothSquare", Float) = 0
		[Toggle]_QuarterCylinder("Quarter Cylinder", Float) = 0
		[Toggle]_HalfCylinder("Half Cylinder", Float) = 0
		[Toggle]_SphereToSquare("SphereToSquare", Float) = 0
		[Toggle]_SphereToSmoothSquare("SphereToSmoothSquare", Float) = 0
		_Rotation("Rotation", Float) = 45
		_Size("Size", Float) = 0.54

	}

	SubShader
	{
		LOD 0

		
		
		ZTest Always
		Cull Off
		ZWrite Off

		
		Pass
		{ 
			CGPROGRAM 

			

			#pragma vertex vert_img_custom 
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			

			struct appdata_img_custom
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
				
			};

			struct v2f_img_custom
			{
				float4 pos : SV_POSITION;
				half2 uv   : TEXCOORD0;
				half2 stereoUV : TEXCOORD2;
		#if UNITY_UV_STARTS_AT_TOP
				half4 uv2 : TEXCOORD1;
				half4 stereoUV2 : TEXCOORD3;
		#endif
				
			};

			uniform sampler2D _MainTex;
			uniform half4 _MainTex_TexelSize;
			uniform half4 _MainTex_ST;
			
			uniform float _SphereToSmoothSquare;
			uniform float _Rotation;
			uniform float _SphereToSquare;
			uniform float _HalfCylinder;
			uniform float _QuarterCylinder;
			uniform float _LogisticSmoothSquare;
			uniform float _LogisticSquare;
			uniform float _LogisticSymmetrical;
			uniform float _LogisticAsymmetrical;
			uniform float _SinSmoothSquare;
			uniform float _SinSquare;
			uniform float _SinSymmetrical;
			uniform float _SinAsymmetrical;
			uniform float _LinearAsymmetrical;
			uniform float _LinearSymmetrical;
			uniform float _LinearSquare;
			uniform float _SmoothDiamond;
			uniform float _Size;


			v2f_img_custom vert_img_custom ( appdata_img_custom v  )
			{
				v2f_img_custom o;
				
				o.pos = UnityObjectToClipPos( v.vertex );
				o.uv = float4( v.texcoord.xy, 1, 1 );

				#if UNITY_UV_STARTS_AT_TOP
					o.uv2 = float4( v.texcoord.xy, 1, 1 );
					o.stereoUV2 = UnityStereoScreenSpaceUVAdjust ( o.uv2, _MainTex_ST );

					if ( _MainTex_TexelSize.y < 0.0 )
						o.uv.y = 1.0 - o.uv.y;
				#endif
				o.stereoUV = UnityStereoScreenSpaceUVAdjust ( o.uv, _MainTex_ST );
				return o;
			}

			half4 frag ( v2f_img_custom i ) : SV_Target
			{
				#ifdef UNITY_UV_STARTS_AT_TOP
					half2 uv = i.uv2;
					half2 stereoUV = i.stereoUV2;
				#else
					half2 uv = i.uv;
					half2 stereoUV = i.stereoUV;
				#endif	
				
				half4 finalColor;


				float2 texCoord140 = i.uv.xy * float2( 2,2 ) + float2( 0,0 );
				float Rotation164 = radians( _Rotation );
				float cos165 = cos( Rotation164 );
				float sin165 = sin( Rotation164 );
				float2 rotator165 = mul( texCoord140 - float2( 1,1 ) , float2x2( cos165 , -sin165 , sin165 , cos165 )) + float2( 1,1 );
				float2 break166 = rotator165;
				float temp_output_141_0 = ( 1.0 - break166.y );
				float temp_output_144_0 = sqrt( saturate( ( 1.0 - ( temp_output_141_0 * temp_output_141_0 ) ) ) );
				float temp_output_136_0 = ( 1.0 - break166.x );
				float temp_output_139_0 = sqrt( saturate( ( 1.0 - ( temp_output_136_0 * temp_output_136_0 ) ) ) );
				float2 texCoord131 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float cos169 = cos( Rotation164 );
				float sin169 = sin( Rotation164 );
				float2 rotator169 = mul( texCoord131 - float2( 0.5,0.5 ) , float2x2( cos169 , -sin169 , sin169 , cos169 )) + float2( 0.5,0.5 );
				float temp_output_135_0 = ( 1.0 - rotator169.x );
				float2 texCoord129 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float cos172 = cos( Rotation164 );
				float sin172 = sin( Rotation164 );
				float2 rotator172 = mul( texCoord129 - float2( 0.5,0.5 ) , float2x2( cos172 , -sin172 , sin172 , cos172 )) + float2( 0.5,0.5 );
				float2 break173 = rotator172;
				float temp_output_125_0 = sin( ( UNITY_PI * break173.y ) );
				float temp_output_193_0 = ( temp_output_125_0 * temp_output_125_0 );
				float temp_output_122_0 = sin( ( UNITY_PI * break173.x ) );
				float temp_output_192_0 = ( temp_output_122_0 * temp_output_122_0 );
				float temp_output_119_0 = sin( ( ( 0.5 * UNITY_PI ) * break173.x ) );
				float2 texCoord19 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float cos175 = cos( Rotation164 );
				float sin175 = sin( Rotation164 );
				float2 rotator175 = mul( texCoord19 - float2( 0.5,0.5 ) , float2x2( cos175 , -sin175 , sin175 , cos175 )) + float2( 0.5,0.5 );
				float2 break176 = rotator175;
				float temp_output_111_0 = sin( ( UNITY_PI * break176.y ) );
				float temp_output_107_0 = sin( ( UNITY_PI * break176.x ) );
				float temp_output_84_0 = ( 1.0 - abs( ( ( break176.x - 0.5 ) * 2.0 ) ) );
				float temp_output_89_0 = ( 1.0 - abs( ( ( break176.y - 0.5 ) * 2.0 ) ) );
				float4 temp_cast_0 = (saturate( (0.0 + (saturate( ( ( ( (( _SphereToSmoothSquare )?( ( temp_output_144_0 * temp_output_139_0 ) ):( 0.0 )) + (( _SphereToSquare )?( min( temp_output_144_0 , temp_output_139_0 ) ):( 0.0 )) + (( _HalfCylinder )?( temp_output_139_0 ):( 0.0 )) + (( _QuarterCylinder )?( sqrt( ( 1.0 - ( temp_output_135_0 * temp_output_135_0 ) ) ) ):( 0.0 )) ) + ( (( _LogisticSmoothSquare )?( ( temp_output_193_0 * temp_output_192_0 ) ):( 0.0 )) + (( _LogisticSquare )?( min( temp_output_193_0 , temp_output_192_0 ) ):( 0.0 )) + (( _LogisticSymmetrical )?( temp_output_192_0 ):( 0.0 )) + (( _LogisticAsymmetrical )?( ( temp_output_119_0 * temp_output_119_0 ) ):( 0.0 )) ) + ( (( _SinSmoothSquare )?( ( temp_output_111_0 * temp_output_107_0 ) ):( 0.0 )) + (( _SinSquare )?( min( temp_output_111_0 , temp_output_107_0 ) ):( 0.0 )) + (( _SinSymmetrical )?( temp_output_107_0 ):( 0.0 )) + (( _SinAsymmetrical )?( sin( ( ( 0.5 * UNITY_PI ) * break176.x ) ) ):( 0.0 )) ) + ( (( _LinearAsymmetrical )?( break176.x ):( 0.0 )) + (( _LinearSymmetrical )?( temp_output_84_0 ):( 0.0 )) + (( _LinearSquare )?( min( temp_output_84_0 , temp_output_89_0 ) ):( 0.0 )) + (( _SmoothDiamond )?( ( temp_output_84_0 * temp_output_89_0 ) ):( 0.0 )) ) ) - ( 1.0 - _Size ) ) ) - 0.0) * (1.0 - 0.0) / (_Size - 0.0)) )).xxxx;
				

				finalColor = temp_cast_0;

				return finalColor;
			} 
			ENDCG 
		}
	}
}