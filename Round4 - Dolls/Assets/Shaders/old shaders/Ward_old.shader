Shader "Old/Ward_Old" {
	//KH: use this for faster anisotropic materials but lower quality
	// has no fresnel effect

	Properties {
		_DiffuseColor ("Diffuse Color", Color) = (1,1,1,1)
		_SpecularColor ("Specular Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_AnisoU ("Anisotropy U", Range(0.0,1.0)) = 0.5
		_AnisoV ("Anisotropy V", Range(0.0,1.0)) = 0.5
		_BumpMap ("Normalmap", 2D) = "bump" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque"}
		//LOD 600
		
		CGPROGRAM
		#pragma surface surf SimpleSpecular addshadow
		#pragma debug
		#pragma target 3.0
		#include "AutoLight.cginc"

		float _AnisoU;
		float _AnisoV;
		sampler2D _MainTex;
		sampler2D _BumpMap;
		fixed4 _DiffuseColor;
		fixed4 _SpecularColor;

		half4 LightingSimpleSpecular (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) {
			// Make sure the interpolated inputs andlightDir
			// constant parameters are normalized
			float3 n = normalize( s.Normal );
			float3 l = normalize( lightDir );
			float3 v = normalize( viewDir );
			float3 h = normalize( l + v );
			float2 fAnisotropicRoughness = float2(_AnisoU, _AnisoV);

			// Apply a small bias to the roughness
			// coefficients to avoid divide-by-zero
			fAnisotropicRoughness += float2( 1e-5f, 1e-5f );
 
			// Define the coordinate frame
			float3 epsilon   = float3( 1.0f, 0.0f, 0.0f );
			float3 tangent   = normalize( cross( n, epsilon ) );
			float3 bitangent = normalize( cross( n, tangent ) );
 
			// Define material properties
			float3 Ps   = _LightColor0.rgb * _SpecularColor.rgb;
 
			// Generate any useful aliases
			float VdotN = dot( v, n );
			float LdotN = dot( l, n );
			float HdotN = dot( h, n );
			float HdotT = dot( h, tangent );
			float HdotB = dot( h, bitangent );
 
			// Evaluate the specular exponent
			float beta_a  = HdotT / fAnisotropicRoughness.x;
			beta_a       *= beta_a;
 
			float beta_b  = HdotB / fAnisotropicRoughness.y;
			beta_b       *= beta_b;
 
			float beta = -2.0f * ( ( beta_a + beta_b ) / ( 1.0f + HdotN ) );
 
			// Evaluate the specular denominator
			float s_den  = 4.0f * 3.14159f; 
			s_den       *= fAnisotropicRoughness.x;
			s_den       *= fAnisotropicRoughness.y;
			s_den       *= sqrt( LdotN * VdotN );
 
			// Compute the final specular term
			float3 Specular = Ps * ( exp( beta ) / s_den );
 
			// Composite the final value:
			return half4( dot( n, l ) * (s.Albedo + Specular) * (atten * 2), 1.0f );


		}

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 worldNormal;
		};
    

		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * _DiffuseColor.rgb;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
