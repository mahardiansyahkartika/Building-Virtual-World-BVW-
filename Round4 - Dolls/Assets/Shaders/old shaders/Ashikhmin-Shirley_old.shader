Shader "Old/Ashikhmin-Shirley_old" {
	//KH: use this for plastic and metals and other anisotropic materials

	Properties {
		_DiffuseColor ("Diffuse Color", Color) = (1,1,1,1)
		_SpecularColor ("Specular Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_AnisoU ("Anisotropy U", Float) = 10.0
		_AnisoV ("Anisotropy V", Float) = 10.0
		_BumpMap ("Normalmap", 2D) = "bump" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" "LightMode"="ForwardBase"}
		//LOD 600
		
		CGPROGRAM
		#pragma surface surf SimpleSpecular 
		#pragma multi_compile_fwdbase
		#pragma target 3.0
		#include "AutoLight.cginc"

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
		ENDCG
	} 
	FallBack "VertexLit"
}
