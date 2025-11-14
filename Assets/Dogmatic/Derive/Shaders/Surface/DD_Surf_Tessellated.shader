//// Derive - Node-Based PBR Texture Editor
// Copyright (c) Dogmatic [admin@dogmatic.tech]

Shader "Derive/Tessellation"
{
	Properties
	{
		_TessPhongStrength( "Phong Tess Strength", Range( 0, 1 ) ) = 0.5
		_Tiling("Tiling", Float) = 1
		[NoScaleOffset][Header(Albedo Settings)]_MainTex("Main Tex", 2D) = "white" {}
		[Toggle]_AlbedoPresent("AlbedoPresent", Float) = 1
		_MainColor("Main Color", Color) = (1,1,1,0)
		[NoScaleOffset][Header(Normal Settings)]_NormalMap("NormalMap", 2D) = "white" {}
		_NormalStrength("Normal Strength", Range( 0 , 1)) = 0
		[NoScaleOffset][Header(Displacement Settings)]_DisplacementMap("Displacement Map", 2D) = "white" {}
		_Displacement("Displacement", Range( 0 , 0.1)) = 0
		_Tessellation("Tessellation", Range( 1 , 80)) = 1
		[NoScaleOffset][Header(AO Settings)]_AmbientOcclusionMap("Ambient Occlusion Map", 2D) = "white" {}
		_AmbientOcclusion("Ambient Occlusion", Range( 0 , 1)) = 0
		[NoScaleOffset][Header(Specular Settings)]_SpecularMap("Specular Map", 2D) = "white" {}
		[Toggle]_SpecularMapPresent("Specular Map Present", Float) = 0
		[Toggle]_SpecularfromRGB("Specular from RGB", Float) = 0
		_SpecularColor("Specular Color", Color) = (0.2924528,0.2910733,0.2910733,0)
		_Gloss("Gloss", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf StandardSpecular keepalpha vertex:vertexDataFunc tessellate:tessFunction tessphong:_TessPhongStrength 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float _Displacement;
		uniform sampler2D _DisplacementMap;
		uniform float _Tiling;
		uniform sampler2D _NormalMap;
		uniform float _NormalStrength;
		uniform float _AlbedoPresent;
		uniform float4 _MainColor;
		uniform sampler2D _MainTex;
		uniform float _SpecularfromRGB;
		uniform float4 _SpecularColor;
		uniform sampler2D _SpecularMap;
		uniform float _SpecularMapPresent;
		uniform float _Gloss;
		uniform sampler2D _AmbientOcclusionMap;
		uniform float _AmbientOcclusion;
		uniform float _Tessellation;
		uniform float _TessPhongStrength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			float4 temp_cast_1 = (_Tessellation).xxxx;
			return temp_cast_1;
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float2 appendResult34 = (float2(_Tiling , _Tiling));
			float2 uv_TexCoord32 = v.texcoord.xy * appendResult34;
			float2 Tiling35 = uv_TexCoord32;
			float grayscale17 = (tex2Dlod( _DisplacementMap, float4( Tiling35, 0, 0.0) ).rgb.r + tex2Dlod( _DisplacementMap, float4( Tiling35, 0, 0.0) ).rgb.g + tex2Dlod( _DisplacementMap, float4( Tiling35, 0, 0.0) ).rgb.b) / 3;
			float3 ase_vertexNormal = v.normal.xyz;
			float3 Displacement26 = ( _Displacement * grayscale17 * ase_vertexNormal );
			v.vertex.xyz += Displacement26;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandardSpecular o )
		{
			float2 appendResult34 = (float2(_Tiling , _Tiling));
			float2 uv_TexCoord32 = i.uv_texcoord * appendResult34;
			float2 Tiling35 = uv_TexCoord32;
			float3 tex2DNode2 = UnpackNormal( tex2D( _NormalMap, Tiling35 ) );
			float3 appendResult51 = (float3(-tex2DNode2.r , -tex2DNode2.g , tex2DNode2.b));
			float3 lerpResult3 = lerp( float3(0,0,1) , appendResult51 , _NormalStrength);
			float3 Normals22 = lerpResult3;
			o.Normal = Normals22;
			float4 Albedo21 = (( _AlbedoPresent )?( tex2D( _MainTex, Tiling35 ) ):( _MainColor ));
			o.Albedo = Albedo21.rgb;
			float4 tex2DNode8 = tex2D( _SpecularMap, Tiling35 );
			float4 Specularity61 = (( _SpecularfromRGB )?( tex2DNode8 ):( _SpecularColor ));
			o.Specular = Specularity61.rgb;
			float Gloss23 = (( _SpecularMapPresent )?( ( _Gloss * tex2DNode8.a ) ):( _Gloss ));
			o.Smoothness = Gloss23;
			float grayscale25 = (tex2D( _AmbientOcclusionMap, Tiling35 ).rgb.r + tex2D( _AmbientOcclusionMap, Tiling35 ).rgb.g + tex2D( _AmbientOcclusionMap, Tiling35 ).rgb.b) / 3;
			float AO24 = ( 1.0 - ( ( 1.0 - grayscale25 ) * _AmbientOcclusion ) );
			o.Occlusion = AO24;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "DeriveUtils.DD_ShaderGUI_Tessellation"
}