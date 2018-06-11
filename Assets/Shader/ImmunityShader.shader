Shader "Custom/ImmunityShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Color2 ("Immunity Color", Color) = (1,1,1,1)
		_ImmunityTex ("Immunity (RGB)", 2D) = "white" {}
		_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
		_RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0
		
		sampler2D _ImmunityTex;

		struct Input {
			float2 uv_ImmunityTex;
			float3 viewDir;
		};
		
		fixed4 _Color;
		fixed4 _Color2;
		float4 _RimColor;
		float _RimPower;

		UNITY_INSTANCING_CBUFFER_START(Props)
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 immunity = tex2D (_ImmunityTex, IN.uv_ImmunityTex) * _Color2;

			o.Albedo = _Color.rgb;
			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
			o.Emission = _RimColor.rgb * pow (rim, sin(_Time.y*3)+3);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
