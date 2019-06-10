Shader "Custom/Water/DiffuseWaterOpaque"
{
	Properties
	{
		_WaterColor ("Water color", Color) = (1, 1, 1, 1)
		_WaterTex ("Water texture", 2D) = "white" {}
		_TextureVisibility("Texture visibility", Range(0, 1)) = 1

		[Space(20)]
		_DistTex ("Distortion", 2D) = "white" {}

		[Space(20)]
		_MoveDirection ("Direction", Vector) = (0.0, 0.0, 0.0, 0.0)
		_TimeScale ("Time Scale", Float) = 1.0
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#pragma multi_compile_fog
			#pragma multi_compile LIGHTMAP_OFF LIGHTMAP_ON

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(3)
				float4 vertex : SV_POSITION;
			};

			sampler2D _WaterTex;
			float4 _WaterTex_ST;

			fixed4 _WaterColor;

			sampler2D _DistTex;
			float4 _DistTex_ST;

			fixed _TextureVisibility;
			fixed _TimeScale;
			fixed2 _MoveDirection;

			v2f vert (appdata v)
			{
				v2f o;

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;

#if defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2)
				UNITY_TRANSFER_FOG(o, o.vertex);
#endif

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 distortion = tex2D(_DistTex, TRANSFORM_TEX(i.uv, _DistTex)) * 2 - 1;
				fixed2 distorted_uv = ((i.uv + distortion.rg) - _TimeScale * _Time.y * _MoveDirection);

				fixed4 waterCol = tex2D(_WaterTex, distorted_uv);
				waterCol = lerp(_WaterColor, fixed4(1, 1, 1, 1), waterCol.r * _TextureVisibility);

				UNITY_APPLY_FOG(i.fogCoord, waterCol);

				return waterCol;
			}
			ENDCG
		}
	}
}
