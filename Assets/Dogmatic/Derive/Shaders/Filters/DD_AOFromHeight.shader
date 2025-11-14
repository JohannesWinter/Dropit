// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

Shader "Hidden/Derive/Filters/AO From Height"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_TextureInput("TextureInput", 2D) = "white" {}
		_Strength("Strength", Float) = 86.25
		_Bias("Bias", Float) = -0.0005

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
			uniform float _Bias;
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


				float2 texCoord25 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float grayscale38 = Luminance(tex2D( _TextureInput, ( ( float2( 1,0 ) * _Bias ) + texCoord25 ) ).rgb);
				float grayscale40 = Luminance(tex2D( _TextureInput, texCoord25 ).rgb);
				float temp_output_28_0 = ( grayscale38 - grayscale40 );
				float grayscale39 = Luminance(tex2D( _TextureInput, ( ( _Bias * float2( 0,1 ) ) + texCoord25 ) ).rgb);
				float temp_output_69_0 = ( abs( temp_output_28_0 ) + abs( ( grayscale39 - grayscale40 ) ) );
				float temp_output_73_0 = ( 1.0 - saturate( ( temp_output_69_0 * _Strength ) ) );
				float4 temp_cast_3 = (temp_output_73_0).xxxx;
				

				finalColor = temp_cast_3;

				return finalColor;
			} 
			ENDCG 
		}
	}
}