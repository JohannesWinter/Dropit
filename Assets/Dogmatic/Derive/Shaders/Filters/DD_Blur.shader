// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

Shader "Hidden/Derive/Filters/Blur"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_TextureInput("TextureInput", 2D) = "white" {}
		_Strength("Strength", Range( 0 , 0.05)) = 0

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
			uniform float _Strength;


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


				float2 texCoord4 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float BlurStrength35 = _Strength;
				float2 appendResult6 = (float2(BlurStrength35 , BlurStrength35));
				float myVarName41 = 0.125;
				float2 texCoord20 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult18 = (float2(-BlurStrength35 , BlurStrength35));
				float2 texCoord23 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult21 = (float2(BlurStrength35 , -BlurStrength35));
				float2 texCoord26 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult24 = (float2(-BlurStrength35 , -BlurStrength35));
				float2 texCoord67 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult70 = (float2(BlurStrength35 , 0.0));
				float2 texCoord71 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult64 = (float2(0.0 , BlurStrength35));
				float2 texCoord68 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult69 = (float2(-BlurStrength35 , 0.0));
				float2 texCoord65 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult66 = (float2(0.0 , -BlurStrength35));
				

				finalColor = ( ( tex2D( _TextureInput, ( texCoord4 + appendResult6 ) ) * myVarName41 ) + ( tex2D( _TextureInput, ( texCoord20 + appendResult18 ) ) * myVarName41 ) + ( tex2D( _TextureInput, ( texCoord23 + appendResult21 ) ) * myVarName41 ) + ( tex2D( _TextureInput, ( texCoord26 + appendResult24 ) ) * myVarName41 ) + ( tex2D( _TextureInput, ( texCoord67 + appendResult70 ) ) * myVarName41 ) + ( tex2D( _TextureInput, ( texCoord71 + appendResult64 ) ) * myVarName41 ) + ( tex2D( _TextureInput, ( texCoord68 + appendResult69 ) ) * myVarName41 ) + ( tex2D( _TextureInput, ( texCoord65 + appendResult66 ) ) * myVarName41 ) );

				return finalColor;
			} 
			ENDCG 
		}
	}
}