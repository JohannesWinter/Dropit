// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

Shader "Hidden/Derive/Data/TexToTex"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_Texture("Texture", 2D) = "white" {}
		[Toggle]_Crop("Crop", Float) = 1
		_OffsetX("OffsetX", Float) = 0
		_OffsetY("OffsetY", Float) = 0
		_TilingX("TilingX", Float) = 1
		_TilingY("TilingY", Float) = 1
		[Toggle]_Darken("Darken", Float) = 0
		_Brightness("Brightness", Float) = 0.5

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
			
			uniform float _Darken;
			uniform sampler2D _Texture;
			uniform float _Crop;
			uniform float _TilingX;
			uniform float _TilingY;
			uniform float _OffsetX;
			uniform float _OffsetY;
			uniform float _Brightness;


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

				// ase common template code
				float2 texCoord8 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult16 = (float2(_TilingX , _TilingY));
				float2 appendResult17 = (float2(_OffsetX , _OffsetY));
				float2 texCoord9 = i.uv.xy * appendResult16 + appendResult17;
				float4 tex2DNode6 = tex2D( _Texture, (( _Crop )?( texCoord9 ):( texCoord8 )) );
				

				finalColor = (( _Darken )?( ( tex2DNode6 * _Brightness ) ):( tex2DNode6 ));

				return finalColor;
			} 
			ENDCG 
		}
	}
}