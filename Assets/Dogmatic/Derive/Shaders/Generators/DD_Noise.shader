// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

Shader "Hidden/Derive/Generators/Noise"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_Scale("Scale", Float) = 1
		_Seed("Seed", Float) = 115.4
		_Tiling("Tiling", Vector) = (5,5,0,0)
		[Toggle]_Turbulence("Turbulence", Float) = 0
		_TextureInput("TextureInput", 2D) = "white" {}
		_Portion("Portion", Float) = 0
		[Toggle]_FirstIteration("First Iteration", Float) = 0
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
			
			uniform float _Turbulence;
			uniform float _FirstIteration;
			uniform sampler2D _TextureInput;
			uniform float4 _TextureInput_ST;
			uniform float2 _Tiling;
			uniform float _Seed;
			uniform float _Scale;
			uniform float _Portion;
			float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }
			float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }
			float snoise( float2 v )
			{
				const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
				float2 i = floor( v + dot( v, C.yy ) );
				float2 x0 = v - i + dot( i, C.xx );
				float2 i1;
				i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
				float4 x12 = x0.xyxy + C.xxzz;
				x12.xy -= i1;
				i = mod2D289( i );
				float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
				float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
				m = m * m;
				m = m * m;
				float3 x = 2.0 * frac( p * C.www ) - 1.0;
				float3 h = abs( x ) - 0.5;
				float3 ox = floor( x + 0.5 );
				float3 a0 = x - ox;
				m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
				float3 g;
				g.x = a0.x * x0.x + h.x * x0.y;
				g.yz = a0.yz * x12.xz + h.yz * x12.yw;
				return 130.0 * dot( m, g );
			}
			
					float2 voronoihash72( float2 p )
					{
						p = p - -3000 * floor( p / -3000 );
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi72( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int k = -3; k <= 3; k++ )
						{
							for ( int i = -3; i <= 3; i++ )
						 	{
						 		float2 g = float2( i, k );
						 		float2 o = voronoihash72( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						
			F1 = 8.0;
			for ( int j = -2; j <= 2; j++ )
			{
			for ( int i = -2; i <= 2; i++ )
			{
			float2 g = mg + float2( i, j );
			float2 o = voronoihash72( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
			float d = dot( 0.5 * ( r + mr ), normalize( r - mr ) );
			F1 = min( F1, d );
			}
			}
			return F1;
					}
			


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
				float4 temp_cast_0 = (0.0).xxxx;
				float temp_output_70_0 = sqrt( _Seed );
				float2 appendResult71 = (float2(temp_output_70_0 , temp_output_70_0));
				float2 texCoord19 = i.uv.xy * _Tiling + appendResult71;
				float simplePerlin2D18 = snoise( texCoord19*_Scale );
				simplePerlin2D18 = simplePerlin2D18*0.5 + 0.5;
				float time72 = 0.0;
				float2 voronoiSmoothId0 = 0;
				float2 coords72 = texCoord19 * _Scale;
				float2 id72 = 0;
				float2 uv72 = 0;
				float fade72 = 0.5;
				float voroi72 = 0;
				float rest72 = 0;
				for( int it72 = 0; it72 <8; it72++ ){
				voroi72 += fade72 * voronoi72( coords72, time72, id72, uv72, 0,voronoiSmoothId0 );
				rest72 += fade72;
				coords72 *= 2;
				fade72 *= 0.5;
				}//Voronoi72
				voroi72 /= rest72;
				float4 temp_cast_1 = (voroi72).xxxx;
				

				finalColor = (( _Turbulence )?( temp_cast_1 ):( ( (( _FirstIteration )?( temp_cast_0 ):( tex2D( _TextureInput, uv_TextureInput ) )) + ( simplePerlin2D18 * _Portion ) ) ));

				return finalColor;
			} 
			ENDCG 
		}
	}
}