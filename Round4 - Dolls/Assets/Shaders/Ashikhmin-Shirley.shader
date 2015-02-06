Shader "Custom/Ashikhmin-Shirley" {
	//KH: use this for faster anisotropic materials but lower quality
	// has no fresnel effect

	Properties {
		_DiffuseColor ("Diffuse Color", Color) = (1,1,1,1)
		_SpecularColor ("Specular Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_AnisoU ("Anisotropy U", Float) = 10.0
		_AnisoV ("Anisotropy V", Float) = 10.0
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
		#pragma fragment frag_surf
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
			half3 n = normalize(s.Normal);
			half3 l = normalize(lightDir);
			half3 v = normalize(viewDir);
			half3 h = normalize( l + v );
			
			half3 epsilon = half3( 1.0f, 0.0f, 0.0f );
			half3 tangent = normalize( cross( n, epsilon ) );
			half3 bitangent = normalize( cross( n, tangent ) );

			float VdotN = dot( v, n );
			float LdotN = dot( l, n );
			float HdotN = dot( h, n );
			float HdotL = dot( h, l );
			float HdotT = dot( h, tangent );
			float HdotB = dot( h, bitangent );

			float3 Rd = s.Albedo;
			float3 Rs = _SpecularColor;
 
			float Nu = _AnisoU;
			float Nv = _AnisoV;
 
			// Compute the diffuse term
			float3 Pd = (28.0f * Rd) / ( 23.0f * 3.14159f );
			Pd *= (1.0f - Rs);
			Pd *= (1.0f - pow(1.0f - (LdotN * 0.5f), 5.0f));
			Pd *= (1.0f - pow(1.0f - (VdotN * 0.5f), 5.0f));
 
			// Compute the specular term
			float ps_num_exp = Nu * HdotT * HdotT + Nv * HdotB * HdotB;
			ps_num_exp /= (1.0f - HdotN * HdotN);
 
			float Ps_num = sqrt( (Nu + 1) * (Nv + 1) );
			Ps_num *= pow( HdotN, ps_num_exp );
 
			float Ps_den = 8.0f * 3.14159f * HdotL;
			Ps_den *= max( LdotN, VdotN );
 
			float3 Ps = Rs * (Ps_num / Ps_den);
			Ps *= ( Rs + (1.0f - Rs) * pow( 1.0f - HdotL, 5.0f ) );
 
			// Composite the final value:
			return half4( (Pd + Ps)  * (atten * 2), 1.0f );


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
			half3 n = normalize(s.Normal);
			half3 l = normalize(lightDir);
			half3 v = normalize(viewDir);
			half3 h = normalize( l + v );
			
			half3 epsilon = half3( 1.0f, 0.0f, 0.0f );
			half3 tangent = normalize( cross( n, epsilon ) );
			half3 bitangent = normalize( cross( n, tangent ) );

			float VdotN = dot( v, n );
			float LdotN = dot( l, n );
			float HdotN = dot( h, n );
			float HdotL = dot( h, l );
			float HdotT = dot( h, tangent );
			float HdotB = dot( h, bitangent );

			float3 Rd = s.Albedo;
			float3 Rs = _LightColor0.rgb;
 
			float Nu = _AnisoU;
			float Nv = _AnisoV;
 
			// Compute the diffuse term
			float3 Pd = (28.0f * Rd) / ( 23.0f * 3.14159f );
			Pd *= (1.0f - Rs);
			Pd *= (1.0f - pow(1.0f - (LdotN * 0.5f), 5.0f));
			Pd *= (1.0f - pow(1.0f - (VdotN * 0.5f), 5.0f));
 
			// Compute the specular term
			float ps_num_exp = Nu * HdotT * HdotT + Nv * HdotB * HdotB;
			ps_num_exp /= (1.0f - HdotN * HdotN);
 
			float Ps_num = sqrt( (Nu + 1) * (Nv + 1) );
			Ps_num *= pow( HdotN, ps_num_exp );
 
			float Ps_den = 8.0f * 3.14159f * HdotL;
			Ps_den *= max( LdotN, VdotN );
 
			float3 Ps = Rs * (Ps_num / Ps_den);
			Ps *= ( Rs + (1.0f - Rs) * pow( 1.0f - HdotL, 5.0f ) );
 
			// Composite the final value:
			return half4( (Pd + Ps)  * (atten * 2), 1.0f );


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
	FallBack "VertexLit"
}
