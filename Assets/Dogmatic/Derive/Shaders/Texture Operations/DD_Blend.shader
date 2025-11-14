// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

Shader "Hidden/Derive/Texture Operations/Blend"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_TextureInput1("TextureInput1", 2D) = "white" {}
		_TextureInput2("TextureInput2", 2D) = "white" {}
		[Toggle]_Subtraction("Subtraction", Float) = 0
		[Toggle]_Difference("Difference", Float) = 0
		[Toggle]_HardMix("Hard Mix", Float) = 0
		[Toggle]_VividLight("Vivid Light", Float) = 0
		[Toggle]_PinLight("Pin Light", Float) = 0
		[Toggle]_HardLight("Hard Light", Float) = 0
		[Toggle]_SoftLight("Soft Light", Float) = 0
		[Toggle]_Lighten("Lighten", Float) = 0
		[Toggle]_Darken("Darken", Float) = 0
		[Toggle]_Exclude("Exclude", Float) = 0
		[Toggle]_Dodge("Dodge", Float) = 0
		[Toggle]_Burn("Burn", Float) = 0
		[Toggle]_Overlay("Overlay", Float) = 0
		[Toggle]_Multiplication("Multiplication", Float) = 0
		[Toggle]_Division("Division", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

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
			
			uniform float _Overlay;
			uniform sampler2D _TextureInput1;
			uniform float4 _TextureInput1_ST;
			uniform sampler2D _TextureInput2;
			uniform float4 _TextureInput2_ST;
			uniform float _Burn;
			uniform float _Dodge;
			uniform float _Exclude;
			uniform float _Darken;
			uniform float _Lighten;
			uniform float _SoftLight;
			uniform float _HardLight;
			uniform float _PinLight;
			uniform float _VividLight;
			uniform float _HardMix;
			uniform float _Difference;
			uniform float _Subtraction;
			uniform float _Multiplication;
			uniform float _Division;


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


				float2 uv_TextureInput1 = i.uv.xy * _TextureInput1_ST.xy + _TextureInput1_ST.zw;
				float4 tex2DNode1 = tex2D( _TextureInput1, uv_TextureInput1 );
				float2 uv_TextureInput2 = i.uv.xy * _TextureInput2_ST.xy + _TextureInput2_ST.zw;
				float4 tex2DNode9 = tex2D( _TextureInput2, uv_TextureInput2 );
				float4 blendOpSrc11 = tex2DNode1;
				float4 blendOpDest11 = tex2DNode9;
				float4 blendOpSrc12 = tex2DNode1;
				float4 blendOpDest12 = tex2DNode9;
				float4 blendOpSrc13 = tex2DNode1;
				float4 blendOpDest13 = tex2DNode9;
				float4 blendOpSrc14 = tex2DNode1;
				float4 blendOpDest14 = tex2DNode9;
				float4 blendOpSrc15 = tex2DNode1;
				float4 blendOpDest15 = tex2DNode9;
				float4 blendOpSrc16 = tex2DNode1;
				float4 blendOpDest16 = tex2DNode9;
				float4 blendOpSrc17 = tex2DNode1;
				float4 blendOpDest17 = tex2DNode9;
				float4 blendOpSrc18 = tex2DNode1;
				float4 blendOpDest18 = tex2DNode9;
				float4 blendOpSrc19 = tex2DNode1;
				float4 blendOpDest19 = tex2DNode9;
				float4 blendOpSrc20 = tex2DNode1;
				float4 blendOpDest20 = tex2DNode9;
				float4 blendOpSrc21 = tex2DNode1;
				float4 blendOpDest21 = tex2DNode9;
				float4 blendOpSrc22 = tex2DNode1;
				float4 blendOpDest22 = tex2DNode9;
				float4 blendOpSrc23 = tex2DNode1;
				float4 blendOpDest23 = tex2DNode9;
				float4 blendOpSrc24 = tex2DNode1;
				float4 blendOpDest24 = tex2DNode9;
				float4 blendOpSrc25 = tex2DNode1;
				float4 blendOpDest25 = tex2DNode9;
				

				finalColor = ( ( (( _Overlay )?( ( saturate( (( blendOpDest11 > 0.5 ) ? ( 1.0 - 2.0 * ( 1.0 - blendOpDest11 ) * ( 1.0 - blendOpSrc11 ) ) : ( 2.0 * blendOpDest11 * blendOpSrc11 ) ) )) ):( float4( 0,0,0,0 ) )) + (( _Burn )?( ( saturate( ( 1.0 - ( ( 1.0 - blendOpDest12) / max( blendOpSrc12, 0.00001) ) ) )) ):( float4( 0,0,0,0 ) )) + (( _Dodge )?( ( saturate( ( blendOpDest13/ max( 1.0 - blendOpSrc13, 0.00001 ) ) )) ):( float4( 0,0,0,0 ) )) + (( _Exclude )?( ( saturate( ( 0.5 - 2.0 * ( blendOpSrc14 - 0.5 ) * ( blendOpDest14 - 0.5 ) ) )) ):( float4( 0,0,0,0 ) )) + (( _Darken )?( ( saturate( min( blendOpSrc15 , blendOpDest15 ) )) ):( float4( 0,0,0,0 ) )) + (( _Lighten )?( ( saturate( 	max( blendOpSrc16, blendOpDest16 ) )) ):( float4( 0,0,0,0 ) )) + (( _SoftLight )?( ( saturate( 2.0f*blendOpDest17*blendOpSrc17 + blendOpDest17*blendOpDest17*(1.0f - 2.0f*blendOpSrc17) )) ):( float4( 0,0,0,0 ) )) + (( _HardLight )?( ( saturate(  (( blendOpSrc18 > 0.5 ) ? ( 1.0 - ( 1.0 - 2.0 * ( blendOpSrc18 - 0.5 ) ) * ( 1.0 - blendOpDest18 ) ) : ( 2.0 * blendOpSrc18 * blendOpDest18 ) ) )) ):( float4( 0,0,0,0 ) )) ) + ( (( _PinLight )?( ( saturate( (( blendOpSrc19 > 0.5 ) ? max( blendOpDest19, 2.0 * ( blendOpSrc19 - 0.5 ) ) : min( blendOpDest19, 2.0 * blendOpSrc19 ) ) )) ):( float4( 0,0,0,0 ) )) + (( _VividLight )?( ( saturate( (( blendOpSrc20 > 0.5 ) ? ( blendOpDest20 / max( ( 1.0 - blendOpSrc20 ) * 2.0 ,0.00001) ) : ( 1.0 - ( ( ( 1.0 - blendOpDest20 ) * 0.5 ) / max( blendOpSrc20,0.00001) ) ) ) )) ):( float4( 0,0,0,0 ) )) + (( _HardMix )?( ( saturate(  round( 0.5 * ( blendOpSrc21 + blendOpDest21 ) ) )) ):( float4( 0,0,0,0 ) )) + (( _Difference )?( ( saturate( abs( blendOpSrc22 - blendOpDest22 ) )) ):( float4( 0,0,0,0 ) )) + (( _Subtraction )?( ( saturate( ( blendOpDest23 - blendOpSrc23 ) )) ):( float4( 0,0,0,0 ) )) + (( _Multiplication )?( ( saturate( ( blendOpSrc24 * blendOpDest24 ) )) ):( float4( 0,0,0,0 ) )) + (( _Division )?( ( saturate( ( blendOpDest25 / max(blendOpSrc25,0.00001) ) )) ):( float4( 0,0,0,0 ) )) ) );

				return finalColor;
			} 
			ENDCG 
		}
	}
}