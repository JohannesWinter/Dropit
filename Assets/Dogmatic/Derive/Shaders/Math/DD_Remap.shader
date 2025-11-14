// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

Shader "Hidden/Derive/Math/Remap"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_TextureInput("TextureInput", 2D) = "white" {}
		_FromOld("FromOld", 2D) = "white" {}
		_FromNew("FromNew", 2D) = "white" {}
		_ToOld("ToOld", 2D) = "white" {}
		_ToNew("ToNew", 2D) = "white" {}
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
			
			uniform sampler2D _TextureInput;
			uniform float4 _TextureInput_ST;
			uniform sampler2D _FromOld;
			uniform float4 _FromOld_ST;
			uniform sampler2D _ToOld;
			uniform float4 _ToOld_ST;
			uniform sampler2D _FromNew;
			uniform float4 _FromNew_ST;
			uniform sampler2D _ToNew;
			uniform float4 _ToNew_ST;


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


				float2 uv_TextureInput = i.uv.xy * _TextureInput_ST.xy + _TextureInput_ST.zw;
				float2 uv_FromOld = i.uv.xy * _FromOld_ST.xy + _FromOld_ST.zw;
				float2 uv_ToOld = i.uv.xy * _ToOld_ST.xy + _ToOld_ST.zw;
				float2 uv_FromNew = i.uv.xy * _FromNew_ST.xy + _FromNew_ST.zw;
				float2 uv_ToNew = i.uv.xy * _ToNew_ST.xy + _ToNew_ST.zw;
				

				finalColor = (tex2D( _FromNew, uv_FromNew ) + (tex2D( _TextureInput, uv_TextureInput ) - tex2D( _FromOld, uv_FromOld )) * (tex2D( _ToNew, uv_ToNew ) - tex2D( _FromNew, uv_FromNew )) / (tex2D( _ToOld, uv_ToOld ) - tex2D( _FromOld, uv_FromOld )));

				return finalColor;
			} 
			ENDCG 
		}
	}
}