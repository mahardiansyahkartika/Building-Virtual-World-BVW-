Shader "Custom/Ward" {
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

	Pass {
		Name "FORWARD"
		Tags { "LightMode" = "ForwardBase" }

		CGPROGRAM
		#pragma vertex vert_surf
		#pragma fragment frag_surf addshadow
		#pragma multi_compile_fwdbase
		#include "HLSLSupport.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#include "AutoLight.cginc"
		#pragma target 3.0

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


		struct v2f_surf {
		  float4 pos : SV_POSITION;
		  float4 pack0 : TEXCOORD0;
		  fixed3 lightDir : TEXCOORD1;
		  fixed3 vlight : TEXCOORD2;
		  float3 viewDir : TEXCOORD3;
		  LIGHTING_COORDS(4,5)
		};

		float4 _MainTex_ST;
		float4 _BumpMap_ST;


		v2f_surf vert_surf (appdata_full v) {
		  v2f_surf o;
		  o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		  o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
		  o.pack0.zw = TRANSFORM_TEX(v.texcoord, _BumpMap);
		  float3 worldN = mul((float3x3)_Object2World, SCALED_NORMAL);
		  TANGENT_SPACE_ROTATION;
		  float3 lightDir = mul (rotation, ObjSpaceLightDir(v.vertex));
		  o.lightDir = lightDir;
		  float3 viewDirForLight = mul (rotation, ObjSpaceViewDir(v.vertex));
		  o.viewDir = viewDirForLight;
		  float3 shlight = ShadeSH9 (float4(worldN,1.0));
		  o.vlight = shlight;
		  TRANSFER_VERTEX_TO_FRAGMENT(o);
		  return o;
		}

	
		fixed4 frag_surf (v2f_surf IN) : COLOR {

		  Input surfIN = (Input)0;
		  surfIN.uv_MainTex = IN.pack0.xy;
		  surfIN.uv_BumpMap = IN.pack0.zw;
		  SurfaceOutput o = (SurfaceOutput)0;
		  o.Albedo = 0.0;
		  o.Emission = 0.0;
		  o.Specular = 0.0;
		  o.Alpha = 0.0;
		  o.Gloss = 0.0;
		  surf (surfIN, o);
		  fixed atten = LIGHT_ATTENUATION(IN);
		  fixed4 c = 0;
		  c = LightingSimpleSpecular (o, IN.lightDir, normalize(half3(IN.viewDir)), atten);
		  c.rgb += o.Albedo * IN.vlight;

		
		  return c;
		}

		ENDCG
		
		
		}




		Pass {
		Name "FORWARD"
		Blend One One
		Tags { "LightMode" = "ForwardAdd" }

		CGPROGRAM
		#pragma vertex vert_surf
		#pragma fragment frag_surf
		#pragma multi_compile_fwdadd
		#include "HLSLSupport.cginc"
		#include "UnityCG.cginc"
		#include "Lighting.cginc"
		#include "AutoLight.cginc"
		#pragma target 3.0
		

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


		struct v2f_surf {
		  float4 pos : SV_POSITION;
		  float4 pack0 : TEXCOORD0;
		  fixed3 lightDir : TEXCOORD1;
		  fixed3 vlight : TEXCOORD2;
		  float3 viewDir : TEXCOORD3;
		  LIGHTING_COORDS(4,5)
		};

		float4 _MainTex_ST;
		float4 _BumpMap_ST;


		v2f_surf vert_surf (appdata_full v) {
		  v2f_surf o;
		  o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
		  o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
		  o.pack0.zw = TRANSFORM_TEX(v.texcoord, _BumpMap);
		  float3 worldN = mul((float3x3)_Object2World, SCALED_NORMAL);
		  TANGENT_SPACE_ROTATION;
		  float3 lightDir = mul (rotation, ObjSpaceLightDir(v.vertex));
		  o.lightDir = lightDir;
		  float3 viewDirForLight = mul (rotation, ObjSpaceViewDir(v.vertex));
		  o.viewDir = viewDirForLight;
		  TRANSFER_VERTEX_TO_FRAGMENT(o);
		  return o;
		}

	
		fixed4 frag_surf (v2f_surf IN) : COLOR {

		  Input surfIN = (Input)0;
		  surfIN.uv_MainTex = IN.pack0.xy;
		  surfIN.uv_BumpMap = IN.pack0.zw;
		  SurfaceOutput o = (SurfaceOutput)0;
		  o.Albedo = 0.0;
		  o.Emission = 0.0;
		  o.Specular = 0.0;
		  o.Alpha = 0.0;
		  o.Gloss = 0.0;
		  surf (surfIN, o);
		  fixed atten = LIGHT_ATTENUATION(IN);
		  fixed4 c = 0;
		  c = LightingSimpleSpecular (o, IN.lightDir, normalize(half3(IN.viewDir)), atten);
		 // c.rgb += o.Albedo * IN.vlight;

		
		  return c;
		}

		ENDCG
		
		
		}

	
	}
}


