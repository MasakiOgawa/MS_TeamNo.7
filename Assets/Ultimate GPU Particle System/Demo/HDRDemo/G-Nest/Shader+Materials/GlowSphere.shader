Shader "VFX/GlowSphere"
{
	Properties
	{
		_LUT("LUT Color", 2D) = "white"{}
		_Color("Color", color) = (1,1,1,1)
		_Offset("Offset", range(0,1)) = 1
		_Falloff("Fall off", range(0.2,4)) = 1
		_ColorBoost("Color boost", float) = 1
	}
	SubShader
	{
		Pass
		{
			Blend one one
			Cull back ZWrite Off ZTest LEqual

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float intensity : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

			fixed4 _Color;
			float _Offset;
			fixed _Falloff;
			sampler2D _LUT;
			float _ColorBoost;

			v2f vert (appdata v)
			{
				v2f o;
				v.vertex.xyz += v.normal * _Offset;
				float3 camVector = normalize(_WorldSpaceCameraPos - v.vertex.xyz);
				o.intensity = pow(1-dot(v.normal, camVector), _Falloff);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				float2 uv = float2(i.intensity, 0.5);
				return tex2D(_LUT, uv) * _Color * _ColorBoost;
			}
			ENDCG
		}
	}
}
