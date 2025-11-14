// Made with Amplif// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

Shader "Hidden/Derive/Mapping/Seamless Mapping"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_TextureInput("TextureInput", 2D) = "white" {}
		_Falloff("Falloff", Float) = 14.89
		[Toggle]_InverseCircularBlend("Inverse Circular Blend", Float) = 0
		[Toggle]_DiamondBlend("Diamond Blend", Float) = 0
		[Toggle]_PolygonalBlend("Polygonal Blend", Float) = 0
		[Toggle]_MirroredEdges("Mirrored Edges", Float) = 0
		[Toggle]_CircularBlend("Circular Blend", Float) = 0

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
			
			uniform float _CircularBlend;
			uniform sampler2D _TextureInput;
			uniform float _Falloff;
			uniform float _InverseCircularBlend;
			uniform float _DiamondBlend;
			uniform float _PolygonalBlend;
			uniform float _MirroredEdges;


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


				float2 texCoord226 = i.uv.xy * float2( 1,1 ) + float2( 0.5,0.5 );
				float2 texCoord8 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 CenteredUV15_g9 = ( i.uv.xy - float2( 0,0 ) );
				float2 _Vector0 = float2(1,0);
				float2 break17_g9 = CenteredUV15_g9;
				float2 appendResult23_g9 = (float2(( length( CenteredUV15_g9 ) * _Vector0.x * 2.0 ) , ( atan2( break17_g9.x , break17_g9.y ) * ( 1.0 / 6.28318548202515 ) * _Vector0.y )));
				float2 CenteredUV15_g10 = ( i.uv.xy - float2( 1,0 ) );
				float2 break17_g10 = CenteredUV15_g10;
				float2 appendResult23_g10 = (float2(( length( CenteredUV15_g10 ) * _Vector0.x * 2.0 ) , ( atan2( break17_g10.x , break17_g10.y ) * ( 1.0 / 6.28318548202515 ) * _Vector0.y )));
				float2 CenteredUV15_g11 = ( i.uv.xy - float2( 0,1 ) );
				float2 break17_g11 = CenteredUV15_g11;
				float2 appendResult23_g11 = (float2(( length( CenteredUV15_g11 ) * _Vector0.x * 2.0 ) , ( atan2( break17_g11.x , break17_g11.y ) * ( 1.0 / 6.28318548202515 ) * _Vector0.y )));
				float2 CenteredUV15_g8 = ( i.uv.xy - float2( 1,1 ) );
				float2 break17_g8 = CenteredUV15_g8;
				float2 appendResult23_g8 = (float2(( length( CenteredUV15_g8 ) * _Vector0.x * 2.0 ) , ( atan2( break17_g8.x , break17_g8.y ) * ( 1.0 / 6.28318548202515 ) * _Vector0.y )));
				float Falloff143 = _Falloff;
				float4 lerpResult225 = lerp( tex2D( _TextureInput, texCoord226 ) , tex2D( _TextureInput, texCoord8 ) , saturate( pow( min( min( appendResult23_g9.x , appendResult23_g10.x ) , min( appendResult23_g11.x , appendResult23_g8.x ) ) , Falloff143 ) ));
				float2 texCoord303 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 texCoord302 = i.uv.xy * float2( 1,1 ) + float2( 0.5,0.5 );
				float2 CenteredUV15_g12 = ( i.uv.xy - float2( 0.5,0.5 ) );
				float2 break17_g12 = CenteredUV15_g12;
				float2 appendResult23_g12 = (float2(( length( CenteredUV15_g12 ) * 1.0 * 2.0 ) , ( atan2( break17_g12.x , break17_g12.y ) * ( 1.0 / 6.28318548202515 ) * 1.0 )));
				float4 lerpResult306 = lerp( tex2D( _TextureInput, texCoord303 ) , tex2D( _TextureInput, texCoord302 ) , saturate( pow( appendResult23_g12.x , Falloff143 ) ));
				float2 texCoord272 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 texCoord271 = i.uv.xy * float2( 1,1 ) + float2( 0.5,0.5 );
				float2 texCoord228 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float cos290 = cos( radians( 45.0 ) );
				float sin290 = sin( radians( 45.0 ) );
				float2 rotator290 = mul( texCoord228 - float2( 0.5,0.5 ) , float2x2( cos290 , -sin290 , sin290 , cos290 )) + float2( 0.5,0.5 );
				float2 break292 = rotator290;
				float temp_output_296_0 = ( 1.0 - ( min( min( break292.x , ( 1.0 - break292.x ) ) , min( break292.y , ( 1.0 - break292.y ) ) ) * 2.0 ) );
				float4 lerpResult270 = lerp( tex2D( _TextureInput, texCoord272 ) , tex2D( _TextureInput, texCoord271 ) , saturate( pow( temp_output_296_0 , Falloff143 ) ));
				float2 texCoord326 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float2 texCoord325 = i.uv.xy * float2( 1,1 ) + float2( 0.5,0.5 );
				float2 texCoord311 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float DiamondMask314 = temp_output_296_0;
				float4 lerpResult329 = lerp( tex2D( _TextureInput, texCoord326 ) , tex2D( _TextureInput, texCoord325 ) , ( saturate( pow( ( 1.0 - ( min( min( texCoord311.x , ( 1.0 - texCoord311.x ) ) , min( texCoord311.y , ( 1.0 - texCoord311.y ) ) ) * 2.0 ) ) , Falloff143 ) ) * saturate( pow( DiamondMask314 , Falloff143 ) ) ));
				float2 texCoord358 = i.uv.xy * float2( 2,2 ) + float2( 0,0 );
				float2 texCoord364 = i.uv.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_367_0 = step( texCoord364.x , 0.5 );
				float temp_output_370_0 = step( 0.5 , texCoord364.y );
				float XX373 = min( temp_output_367_0 , temp_output_370_0 );
				float2 appendResult360 = (float2(( 1.0 - texCoord358.x ) , texCoord358.y));
				float temp_output_376_0 = step( 0.5 , texCoord364.x );
				float XY378 = min( temp_output_370_0 , temp_output_376_0 );
				float2 appendResult361 = (float2(texCoord358.x , ( 1.0 - texCoord358.y )));
				float temp_output_374_0 = step( texCoord364.y , 0.5 );
				float YX381 = min( temp_output_367_0 , temp_output_374_0 );
				float YY380 = min( temp_output_376_0 , temp_output_374_0 );
				

				finalColor = ( (( _CircularBlend )?( lerpResult225 ):( float4( 0,0,0,0 ) )) + (( _InverseCircularBlend )?( lerpResult306 ):( float4( 0,0,0,0 ) )) + (( _DiamondBlend )?( lerpResult270 ):( float4( 0,0,0,0 ) )) + (( _PolygonalBlend )?( lerpResult329 ):( float4( 0,0,0,0 ) )) + (( _MirroredEdges )?( ( ( tex2D( _TextureInput, texCoord358 ) * XX373 ) + ( tex2D( _TextureInput, appendResult360 ) * XY378 ) + ( tex2D( _TextureInput, appendResult361 ) * YX381 ) + ( tex2D( _TextureInput, ( 1.0 - texCoord358 ) ) * YY380 ) ) ):( float4( 0,0,0,0 ) )) );

				return finalColor;
			} 
			ENDCG 
		}
	}
}