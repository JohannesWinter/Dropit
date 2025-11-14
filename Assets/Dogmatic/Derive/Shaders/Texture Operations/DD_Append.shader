// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

Shader "Hidden/Derive/Texture Operations/Append"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_TextureInput1("TextureInput1", 2D) = "white" {}
		_TextureInput2("TextureInput2", 2D) = "white" {}
		_TextureInput3("TextureInput3", 2D) = "white" {}
		_TextureInput4("TextureInput4", 2D) = "white" {}
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
			
			uniform sampler2D _TextureInput1;
			uniform float4 _TextureInput1_ST;
			uniform sampler2D _TextureInput2;
			uniform float4 _TextureInput2_ST;
			uniform sampler2D _TextureInput3;
			uniform float4 _TextureInput3_ST;
			uniform sampler2D _TextureInput4;
			uniform float4 _TextureInput4_ST;


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
				float2 uv_TextureInput2 = i.uv.xy * _TextureInput2_ST.xy + _TextureInput2_ST.zw;
				float2 uv_TextureInput3 = i.uv.xy * _TextureInput3_ST.xy + _TextureInput3_ST.zw;
				float2 uv_TextureInput4 = i.uv.xy * _TextureInput4_ST.xy + _TextureInput4_ST.zw;
				float4 appendResult10 = (float4(tex2D( _TextureInput1, uv_TextureInput1 ).r , tex2D( _TextureInput2, uv_TextureInput2 ).r , tex2D( _TextureInput3, uv_TextureInput3 ).r , tex2D( _TextureInput4, uv_TextureInput4 ).r));
				

				finalColor = appendResult10;

				return finalColor;
			} 
			ENDCG 
		}
	}
}