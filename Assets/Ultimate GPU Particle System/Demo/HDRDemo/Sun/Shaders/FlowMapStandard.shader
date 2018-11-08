Shader "Custom/FlowMapStandard" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MotionVectors("Flow (RGB)", 2D) = "white" {}
		_MotionVectorStrength("MotionVectorStrength", Range(0,1)) = 0.5
		_MotionVectorSpeed("MotionVectorSpeed", Range(0,10)) = 0.5
		_EmissiveStrength("Emission strength", Range(0,10)) = 0.5
	}
	SubShader {
		Tags { "RenderQueue"="3000" }
		Zwrite On ZTest LEqual Cull back

		CGPROGRAM
		#pragma surface surf Standard
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MotionVectors;

		struct Input {
			float2 uv_MainTex;
			float2 uv_MotionVectors;
			float3 color;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		float _MotionVectorStrength;
		float _MotionVectorSpeed;
		float _EmissiveStrength;

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float progress = (_Time.y * _MotionVectorSpeed) % 1;

			float2 flow = tex2D(_MotionVectors, IN.uv_MotionVectors).rg * 2 - 1;
			float4 col1 = tex2D(_MainTex, IN.uv_MainTex + lerp(float2(0, 0), flow, progress-1) * _MotionVectorStrength);
			float4 col2 = tex2D(_MainTex, IN.uv_MainTex + lerp(float2(0, 0), flow, progress) * _MotionVectorStrength);

			float4 c = lerp(col2, col1, progress) * _Color;

			o.Albedo = c;
			o.Emission = c * _EmissiveStrength;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
