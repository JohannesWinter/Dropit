// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

Shader "Hidden/Derive/Generators/Voronoi"
{
	Properties
	{
		_MainTex ( "Screen", 2D ) = "black" {}
		_Scale("Scale", Float) = 1
		_Seed("Seed", Float) = 115.4
		_Tiling("Tiling", Vector) = (5,5,0,0)
		_Octaves("Octaves", Int) = 4
		[Toggle]_EuclidianSoft("EuclidianSoft", Float) = 0
		[Toggle]_EuclidianStucco("EuclidianStucco", Float) = 0
		[Toggle]_ManhattanSoft("ManhattanSoft", Float) = 0
		[Toggle]_ManhattanStucco("ManhattanStucco", Float) = 0
		[Toggle]_Simple("Simple", Float) = 0

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
			
			uniform float _EuclidianSoft;
			uniform float _Scale;
			uniform float2 _Tiling;
			uniform float _Seed;
			uniform int _Octaves;
			uniform float _EuclidianStucco;
			uniform float _ManhattanSoft;
			uniform float _ManhattanStucco;
			uniform float _Simple;
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
						for ( int j = -3; j <= 3; j++ )
						{
							for ( int i = -3; i <= 3; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash72( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.707 * sqrt(dot( r, r ));
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F2 - F1;
					}
			
					float2 voronoihash82( float2 p )
					{
						p = p - -3000 * floor( p / -3000 );
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi82( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash82( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.707 * sqrt(dot( r, r ));
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			
					float2 voronoihash85( float2 p )
					{
						p = p - -3000 * floor( p / -3000 );
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi85( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -3; j <= 3; j++ )
						{
							for ( int i = -3; i <= 3; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash85( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * ( abs(r.x) + abs(r.y) );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F2 - F1;
					}
			
					float2 voronoihash86( float2 p )
					{
						p = p - -3000 * floor( p / -3000 );
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi86( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash86( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * ( abs(r.x) + abs(r.y) );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			
					float2 voronoihash106( float2 p )
					{
						p = p - -3000 * floor( p / -3000 );
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi106( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -3; j <= 3; j++ )
						{
							for ( int i = -3; i <= 3; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash106( n + g );
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
			for ( int k = -2; k <= 2; k++ )
			{
			for ( int i = -2; i <= 2; i++ )
			{
			float2 g = mg + float2( i, k );
			float2 o = voronoihash106( n + g );
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


				float time72 = 0.0;
				float2 voronoiSmoothId0 = 0;
				float temp_output_70_0 = sqrt( _Seed );
				float2 appendResult71 = (float2(temp_output_70_0 , temp_output_70_0));
				float2 texCoord19 = i.uv.xy * _Tiling + appendResult71;
				float2 coords72 = texCoord19 * _Scale;
				float2 id72 = 0;
				float2 uv72 = 0;
				float fade72 = 0.5;
				float voroi72 = 0;
				float rest72 = 0;
				for( int it72 = 0; it72 < _Octaves; it72++ ){
				voroi72 += fade72 * voronoi72( coords72, time72, id72, uv72, 0,voronoiSmoothId0 );
				rest72 += fade72;
				coords72 *= 2;
				fade72 *= 0.5;
				}//Voronoi72
				voroi72 /= rest72;
				float time82 = 0.0;
				float2 coords82 = texCoord19 * _Scale;
				float2 id82 = 0;
				float2 uv82 = 0;
				float fade82 = 0.5;
				float voroi82 = 0;
				float rest82 = 0;
				for( int it82 = 0; it82 < _Octaves; it82++ ){
				voroi82 += fade82 * voronoi82( coords82, time82, id82, uv82, 0,voronoiSmoothId0 );
				rest82 += fade82;
				coords82 *= 2;
				fade82 *= 0.5;
				}//Voronoi82
				voroi82 /= rest82;
				float time85 = 0.0;
				float2 coords85 = texCoord19 * _Scale;
				float2 id85 = 0;
				float2 uv85 = 0;
				float fade85 = 0.5;
				float voroi85 = 0;
				float rest85 = 0;
				for( int it85 = 0; it85 < _Octaves; it85++ ){
				voroi85 += fade85 * voronoi85( coords85, time85, id85, uv85, 0,voronoiSmoothId0 );
				rest85 += fade85;
				coords85 *= 2;
				fade85 *= 0.5;
				}//Voronoi85
				voroi85 /= rest85;
				float time86 = 0.0;
				float2 coords86 = texCoord19 * _Scale;
				float2 id86 = 0;
				float2 uv86 = 0;
				float fade86 = 0.5;
				float voroi86 = 0;
				float rest86 = 0;
				for( int it86 = 0; it86 < _Octaves; it86++ ){
				voroi86 += fade86 * voronoi86( coords86, time86, id86, uv86, 0,voronoiSmoothId0 );
				rest86 += fade86;
				coords86 *= 2;
				fade86 *= 0.5;
				}//Voronoi86
				voroi86 /= rest86;
				float time106 = 0.0;
				float2 coords106 = texCoord19 * _Scale;
				float2 id106 = 0;
				float2 uv106 = 0;
				float fade106 = 0.5;
				float voroi106 = 0;
				float rest106 = 0;
				for( int it106 = 0; it106 < _Octaves; it106++ ){
				voroi106 += fade106 * voronoi106( coords106, time106, id106, uv106, 0,voronoiSmoothId0 );
				rest106 += fade106;
				coords106 *= 2;
				fade106 *= 0.5;
				}//Voronoi106
				voroi106 /= rest106;
				float4 temp_cast_0 = (( (( _EuclidianSoft )?( voroi72 ):( 0.0 )) + (( _EuclidianStucco )?( voroi82 ):( 0.0 )) + (( _ManhattanSoft )?( voroi85 ):( 0.0 )) + (( _ManhattanStucco )?( voroi86 ):( 0.0 )) + (( _Simple )?( voroi106 ):( 0.0 )) )).xxxx;
				

				finalColor = temp_cast_0;

				return finalColor;
			} 
			ENDCG 
		}
	}
}