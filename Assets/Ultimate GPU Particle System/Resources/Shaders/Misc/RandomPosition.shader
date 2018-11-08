Shader "FX/SkinnedMeshSquare"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
		Zwrite on ZTest Always Cull back
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
			};

			struct v2f
			{
				float4 color : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.color = mul(v.vertex, unity_WorldToObject);//mul(v.vertex, unity_ObjectToWorld);
				float4 pos = float4( v.uv2.x * 2 - 1, v.uv2.y * 2 - 1,0, 1);
				//o.vertex = UnityObjectToClipPos(pos);
				o.vertex = pos;//UV to clip space 
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				float4 col = i.color;
				col.a = 1;
				return col;
			}
			ENDCG
		}
	}
}
