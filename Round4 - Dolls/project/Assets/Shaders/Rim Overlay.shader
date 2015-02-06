Shader "Custom/Rim_Overlay" {
	Properties {
      _RimColor ("Rim Color", Color) = (1.0,1.0,0.5,1.0)
      _RimPower ("Rim Power", Range(0.1,8.0)) = 1.0
    }

	
	
	
	SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="Transparent"}
	Blend one one
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float3 normalDir : TEXCOORD0;
				float4 worldpos : TEXCOORD1;
			};

			float4 _RimColor;
			float _RimPower;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.worldpos = mul(_Object2World, v.vertex);
				o.normalDir = normalize( mul(float4(v.normal, 0.0), _World2Object).xyz);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldpos.xyz);
				half rim = min(max(1.0 - saturate((dot (normalize(viewDir), i.normalDir))),0.3),0.6);
				fixed4 col = fixed4(_RimColor.rgb * pow (rim, _RimPower),0.0);
				return col;
			}
		ENDCG
	}
	
    }
}
