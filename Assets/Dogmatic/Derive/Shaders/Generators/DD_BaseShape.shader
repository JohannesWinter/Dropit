// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

Shader "Hidden/Derive/Generators/BaseShape"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_Radius("Radius", Float) = 0.5
		[Toggle]_RoundedSquare("RoundedSquare", Float) = 0
		[Toggle]_Circle("Circle", Float) = 0
		[Toggle]_Square("Square", Float) = 0
		_Ridge("Ridge", Float) = 0.43
		[Toggle]_Octagon("Octagon", Float) = 1
		_HexagonFalloff("HexagonFalloff", Float) = 0
		[Toggle]_Hexagon("Hexagon", Float) = 0
		[Toggle]_Triangle("Triangle", Float) = 0
		_TriangleFalloff("TriangleFalloff", Float) = 4.22

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
			
			uniform float _Square;
			uniform float _RoundedSquare;
			uniform float _Radius;
			uniform float _Circle;
			uniform float _Octagon;
			uniform float _Ridge;
			uniform float _Hexagon;
			uniform float _HexagonFalloff;
			uniform float _Triangle;
			uniform float _TriangleFalloff;


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


				float SideLength174 = ( _Radius / 2.0 );
				float2 texCoord157 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float RoundedCornerRadius135 = ( ( 1.0 - _Radius ) / 2.0 );
				float2 appendResult153 = (float2(RoundedCornerRadius135 , RoundedCornerRadius135));
				float2 texCoord130 = i.uv.xy * float2( 2,2 ) + ( -appendResult153 + float2( -1,-1 ) );
				float2 CenteredUV15_g7 = ( texCoord130 - float2( 0.5,0.5 ) );
				float temp_output_133_0 = ( 1.0 / _Radius );
				float2 break17_g7 = CenteredUV15_g7;
				float2 appendResult23_g7 = (float2(( length( CenteredUV15_g7 ) * temp_output_133_0 * 2.0 ) , ( atan2( break17_g7.x , break17_g7.y ) * ( 1.0 / 6.28318548202515 ) * 0.0 )));
				float grayscale127 = Luminance(float3( appendResult23_g7 ,  0.0 ));
				float2 appendResult148 = (float2(RoundedCornerRadius135 , ( -RoundedCornerRadius135 + -1.0 )));
				float2 texCoord125 = i.uv.xy * float2( 2,2 ) + appendResult148;
				float2 CenteredUV15_g8 = ( texCoord125 - float2( 0.5,0.5 ) );
				float2 break17_g8 = CenteredUV15_g8;
				float2 appendResult23_g8 = (float2(( length( CenteredUV15_g8 ) * temp_output_133_0 * 2.0 ) , ( atan2( break17_g8.x , break17_g8.y ) * ( 1.0 / 6.28318548202515 ) * 0.0 )));
				float grayscale122 = Luminance(float3( appendResult23_g8 ,  0.0 ));
				float2 appendResult140 = (float2(( -RoundedCornerRadius135 + -1.0 ) , RoundedCornerRadius135));
				float2 texCoord118 = i.uv.xy * float2( 2,2 ) + appendResult140;
				float2 CenteredUV15_g5 = ( texCoord118 - float2( 0.5,0.5 ) );
				float2 break17_g5 = CenteredUV15_g5;
				float2 appendResult23_g5 = (float2(( length( CenteredUV15_g5 ) * temp_output_133_0 * 2.0 ) , ( atan2( break17_g5.x , break17_g5.y ) * ( 1.0 / 6.28318548202515 ) * 0.0 )));
				float grayscale116 = Luminance(float3( appendResult23_g5 ,  0.0 ));
				float2 appendResult137 = (float2(RoundedCornerRadius135 , RoundedCornerRadius135));
				float2 texCoord113 = i.uv.xy * float2( 2,2 ) + appendResult137;
				float2 CenteredUV15_g6 = ( texCoord113 - float2( 0.5,0.5 ) );
				float2 break17_g6 = CenteredUV15_g6;
				float2 appendResult23_g6 = (float2(( length( CenteredUV15_g6 ) * temp_output_133_0 * 2.0 ) , ( atan2( break17_g6.x , break17_g6.y ) * ( 1.0 / 6.28318548202515 ) * 0.0 )));
				float grayscale106 = Luminance(float3( appendResult23_g6 ,  0.0 ));
				float RoundCorners156 = ( step( 0.78 , ( 1.0 - grayscale127 ) ) + step( 0.78 , ( 1.0 - grayscale122 ) ) + step( 0.78 , ( 1.0 - grayscale116 ) ) + step( 0.78 , ( 1.0 - grayscale106 ) ) );
				float RoundedCube175 = max( max( step( SideLength174 , ( 1.0 - abs( ( ( texCoord157.x - 0.5 ) * 2.0 ) ) ) ) , step( SideLength174 , ( 1.0 - abs( ( ( texCoord157.y - 0.5 ) * 2.0 ) ) ) ) ) , RoundCorners156 );
				float2 CenteredUV15_g9 = ( i.uv.xy - float2( 0.5,0.5 ) );
				float2 break17_g9 = CenteredUV15_g9;
				float2 appendResult23_g9 = (float2(( length( CenteredUV15_g9 ) * 1.0 * 2.0 ) , ( atan2( break17_g9.x , break17_g9.y ) * ( 1.0 / 6.28318548202515 ) * 0.0 )));
				float grayscale177 = Luminance(float3( appendResult23_g9 ,  0.0 ));
				float Circle181 = step( 0.78 , ( 1.0 - grayscale177 ) );
				float Ridge216 = ( _Ridge / 3.0003 );
				float2 texCoord188 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float grayscale190 = (float3( texCoord188 ,  0.0 ).r + float3( texCoord188 ,  0.0 ).g + float3( texCoord188 ,  0.0 ).b) / 3;
				float2 texCoord197 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float cos210 = cos( radians( 90.0 ) );
				float sin210 = sin( radians( 90.0 ) );
				float2 rotator210 = mul( texCoord197 - float2( 0.5,0.5 ) , float2x2( cos210 , -sin210 , sin210 , cos210 )) + float2( 0.5,0.5 );
				float grayscale205 = (float3( rotator210 ,  0.0 ).r + float3( rotator210 ,  0.0 ).g + float3( rotator210 ,  0.0 ).b) / 3;
				float2 texCoord198 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float cos217 = cos( radians( 180.0 ) );
				float sin217 = sin( radians( 180.0 ) );
				float2 rotator217 = mul( texCoord198 - float2( 0.5,0.5 ) , float2x2( cos217 , -sin217 , sin217 , cos217 )) + float2( 0.5,0.5 );
				float grayscale222 = (float3( rotator217 ,  0.0 ).r + float3( rotator217 ,  0.0 ).g + float3( rotator217 ,  0.0 ).b) / 3;
				float2 texCoord199 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float cos229 = cos( radians( 270.0 ) );
				float sin229 = sin( radians( 270.0 ) );
				float2 rotator229 = mul( texCoord199 - float2( 0.5,0.5 ) , float2x2( cos229 , -sin229 , sin229 , cos229 )) + float2( 0.5,0.5 );
				float grayscale230 = (float3( rotator229 ,  0.0 ).r + float3( rotator229 ,  0.0 ).g + float3( rotator229 ,  0.0 ).b) / 3;
				float Octagon238 = ( 1.0 - ( step( 0.0 , ( Ridge216 + ( grayscale190 - 0.6666 ) ) ) + step( 0.0 , ( Ridge216 + ( grayscale205 - 0.666 ) ) ) + step( 0.0 , ( Ridge216 + ( grayscale222 - 0.666 ) ) ) + step( 0.0 , ( Ridge216 + ( grayscale230 - 0.666 ) ) ) ) );
				float2 texCoord247 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float HexagonSlope252 = -_HexagonFalloff;
				float cos250 = cos( radians( HexagonSlope252 ) );
				float sin250 = sin( radians( HexagonSlope252 ) );
				float2 rotator250 = mul( texCoord247 - float2( 1,0.5 ) , float2x2( cos250 , -sin250 , sin250 , cos250 )) + float2( 1,0.5 );
				float grayscale243 = (float3( rotator250 ,  0.0 ).r + float3( rotator250 ,  0.0 ).g + float3( rotator250 ,  0.0 ).b) / 3;
				float2 texCoord257 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult266 = (float2(( 1.0 - texCoord257.x ) , texCoord257.y));
				float cos258 = cos( radians( HexagonSlope252 ) );
				float sin258 = sin( radians( HexagonSlope252 ) );
				float2 rotator258 = mul( appendResult266 - float2( 1,0.5 ) , float2x2( cos258 , -sin258 , sin258 , cos258 )) + float2( 1,0.5 );
				float grayscale254 = (float3( rotator258 ,  0.0 ).r + float3( rotator258 ,  0.0 ).g + float3( rotator258 ,  0.0 ).b) / 3;
				float2 texCoord277 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult275 = (float2(texCoord277.x , ( 1.0 - texCoord277.y )));
				float cos270 = cos( radians( HexagonSlope252 ) );
				float sin270 = sin( radians( HexagonSlope252 ) );
				float2 rotator270 = mul( appendResult275 - float2( 1,0.5 ) , float2x2( cos270 , -sin270 , sin270 , cos270 )) + float2( 1,0.5 );
				float grayscale268 = (float3( rotator270 ,  0.0 ).r + float3( rotator270 ,  0.0 ).g + float3( rotator270 ,  0.0 ).b) / 3;
				float2 texCoord286 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float cos280 = cos( radians( HexagonSlope252 ) );
				float sin280 = sin( radians( HexagonSlope252 ) );
				float2 rotator280 = mul( ( 1.0 - texCoord286 ) - float2( 1,0.5 ) , float2x2( cos280 , -sin280 , sin280 , cos280 )) + float2( 1,0.5 );
				float grayscale278 = (float3( rotator280 ,  0.0 ).r + float3( rotator280 ,  0.0 ).g + float3( rotator280 ,  0.0 ).b) / 3;
				float Hexagon290 = ( 1.0 - ( step( 0.0 , ( 0.1515 + ( grayscale243 - 0.651 ) ) ) + step( 0.0 , ( 0.1515 + ( grayscale254 - 0.651 ) ) ) + step( 0.0 , ( 0.1515 + ( grayscale268 - 0.651 ) ) ) + step( 0.0 , ( 0.1515 + ( grayscale278 - 0.651 ) ) ) ) );
				float2 texCoord295 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float TriangleFalloff294 = -_TriangleFalloff;
				float cos298 = cos( radians( TriangleFalloff294 ) );
				float sin298 = sin( radians( TriangleFalloff294 ) );
				float2 rotator298 = mul( texCoord295 - float2( 0,0 ) , float2x2( cos298 , -sin298 , sin298 , cos298 )) + float2( 0,0 );
				float3 temp_cast_13 = (rotator298.x).xxx;
				float grayscale296 = (temp_cast_13.r + temp_cast_13.g + temp_cast_13.b) / 3;
				float2 texCoord303 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 appendResult314 = (float2(( 1.0 - texCoord303.x ) , texCoord303.y));
				float cos304 = cos( radians( TriangleFalloff294 ) );
				float sin304 = sin( radians( TriangleFalloff294 ) );
				float2 rotator304 = mul( appendResult314 - float2( 0,0 ) , float2x2( cos304 , -sin304 , sin304 , cos304 )) + float2( 0,0 );
				float3 temp_cast_14 = (rotator304.x).xxx;
				float grayscale308 = (temp_cast_14.r + temp_cast_14.g + temp_cast_14.b) / 3;
				float Triangle317 = min( step( 0.0 , grayscale296 ) , step( 0.0 , grayscale308 ) );
				float4 temp_cast_15 = (( (( _Square )?( 1.0 ):( 0.0 )) + (( _RoundedSquare )?( RoundedCube175 ):( 0.0 )) + (( _Circle )?( Circle181 ):( 0.0 )) + (( _Octagon )?( Octagon238 ):( 0.0 )) + (( _Hexagon )?( Hexagon290 ):( 0.0 )) + (( _Triangle )?( Triangle317 ):( 0.0 )) )).xxxx;
				

				finalColor = temp_cast_15;

				return finalColor;
			} 
			ENDCG 
		}
	}
}