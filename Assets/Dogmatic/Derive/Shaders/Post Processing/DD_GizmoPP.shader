// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

Shader "Hidden/Derive/Post Processing/Preview Post Processing"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_Tex("Tex", 2D) = "white" {}
		_Tex2("Tex2", 2D) = "white" {}
		_gizmoTexTilingX("gizmoTexTilingX", Float) = 5
		_gizmoTexOffset("gizmoTexOffset", Float) = 0
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
			
			uniform sampler2D _Tex;
			uniform float4 _Tex_ST;
			uniform sampler2D _Tex2;
			uniform float _gizmoTexTilingX;
			uniform float _gizmoTexOffset;


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


				float2 uv_Tex = i.uv.xy * _Tex_ST.xy + _Tex_ST.zw;
				float2 appendResult52 = (float2(_gizmoTexTilingX , 5.0));
				float2 appendResult54 = (float2(-_gizmoTexOffset , 0.0));
				float2 texCoord34 = i.uv.xy * appendResult52 + appendResult54;
				float4 tex2DNode19 = tex2D( _Tex2, texCoord34 );
				float4 lerpResult22 = lerp( tex2D( _Tex, uv_Tex ) , tex2DNode19 , ( 1.0 - step( ( ( tex2DNode19.r + tex2DNode19.g + tex2DNode19.b ) / 3.0 ) , 0.0 ) ));
				

				finalColor = lerpResult22;

				return finalColor;
			} 
			ENDCG 
		}
	}
}