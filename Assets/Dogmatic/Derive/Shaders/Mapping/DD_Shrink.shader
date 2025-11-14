// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

Shader "Hidden/Derive/Mapping/Shrink"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_TextureInput("TextureInput", 2D) = "white" {}
		_OffsetX("OffsetX", Float) = 1.5
		_OffsetY("OffsetY", Float) = -2.5
		_TilingX("TilingX", Float) = 2.32
		_TilingY("TilingY", Float) = 2
		_ExtentX("ExtentX", Float) = 1
		_ExtentY("ExtentY", Float) = 1

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
			
			uniform sampler2D _TextureInput;
			uniform float _TilingX;
			uniform float _TilingY;
			uniform float _OffsetX;
			uniform float _ExtentX;
			uniform float _OffsetY;
			uniform float _ExtentY;


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


				float2 appendResult66 = (float2(( _TilingX / 2.0 ) , ( _TilingY / 2.0 )));
				float2 appendResult57 = (float2(_TilingX , _TilingY));
				float2 texCoord8 = i.uv.xy * appendResult57 + float2( 0,0 );
				float temp_output_53_0 = ( _ExtentX / 2.0 );
				float extentX43 = temp_output_53_0;
				float temp_output_52_0 = ( _ExtentY / 2.0 );
				float extentY44 = temp_output_52_0;
				float4 lerpResult12 = lerp( float4( 0,0,0,0 ) , tex2D( _TextureInput, ( -( appendResult66 + float2( 0.5,0.5 ) ) + texCoord8 ) ) , min( step( abs( ( texCoord8.x - ( _OffsetX + extentX43 ) ) ) , temp_output_53_0 ) , step( abs( ( -texCoord8.y - ( _OffsetY + extentY44 ) ) ) , temp_output_52_0 ) ));
				

				finalColor = lerpResult12;

				return finalColor;
			} 
			ENDCG 
		}
	}
}