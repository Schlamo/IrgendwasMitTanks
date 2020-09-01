Shader "Unlit/TShader"
{
	Properties
	{
		_MainTex ("Main Texture", 2D)	= "white" {}
		_RocksTex ("Slope Texture", 2D) = "white" {}
		_min("Main Amplifier", Range(0, 1.0)) = 0
		_max("Slope Amplifier", Range(0, 1.0)) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float slope : ANGLE;
			};

			sampler2D _MainTex;
			sampler2D _RocksTex;
			float _min;
			float _max;
			float4 _MainTex_ST;
			float4 _RocksTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv2, _RocksTex);
				float3 up = { 0.0f, 1.0f, 0.0f };
				float3 ve = v.normal;
				o.slope = dot(normalize(ve), up) / (length(up) * length(ve));
				o.slope = acos(o.slope);
				float min = 0.125f;
				float max = 0.25f;
				o.slope = clamp((o.slope- _min) / (_max - _min), 0.0f, 1.0f);
				return o;
			}

			float4 frag (v2f i) : SV_Target
			{
			float rockIntensity = i.slope;
			float grassIntensity = 1.0f - rockIntensity;

			float3 grassTex = tex2D(_MainTex, i.uv) * grassIntensity;
			float3 rockTex = tex2D(_RocksTex, i.uv2) * rockIntensity;

			float4 color = { grassTex+rockTex, 1.0f};
			return color;
			}
			ENDCG
		}
	}
}
