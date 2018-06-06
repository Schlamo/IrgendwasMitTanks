Shader "Custom/TerrainShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Color2 ("Pitch Color", Color) = (1,1,1,1)
		_Amplifier ("Pitch Amplifier", Range(0, 2)) = 1
		_Factor ("Factor", Range(0, 10)) = 1
		_MainTex ("Primary Color (RGB)", 2D) = "white" {}
		_SecondTex ("Secondary Color (RGB)", 2D) = "white" {}
		_ThirdTex ("MixMap", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		
		sampler2D _MainTex;
		sampler2D _SecondTex;
		sampler2D _ThirdTex;

		struct Input {
			float2 uv_MainTex;
			float2 uv_SecondTex;
			float2 uv_ThirdTex;
		};

		half _Glossiness;
		half _Metallic;
		half _Factor;
		half _Amplifier;
		fixed4 _Color;
		fixed4 _Color2;

		float3 clamp(float v, float3 v2) {
			float3 v1 = float3(v, v, v);
			float r = v1.x-v2.x;
			float g = v1.y-v2.y;
			float b = v1.z-v2.z;
			if(r < 0) {
				r = 0;
			}
			if(g < 0) {
				g = 0;
			}
			if(b < 0) {
				b = 0;
			}
			return float3(r,g,b);
		}

		UNITY_INSTANCING_CBUFFER_START(Props)
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 meadow = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 pitch = tex2D (_SecondTex, IN.uv_SecondTex) * _Color2;
			fixed4 stuff = tex2D (_ThirdTex, IN.uv_ThirdTex);
			
			fixed3 meadowAmp = stuff;
			fixed3 pitchAmp = clamp(_Amplifier, stuff);
			

			o.Albedo = meadow.rgb * (1-pitchAmp) + pitch.rgb * pitchAmp* _Factor;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = meadow.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
