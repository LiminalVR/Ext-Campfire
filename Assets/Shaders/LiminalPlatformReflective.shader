Shader "Liminal/Environment/Platform (Reflective)"
{
	Properties
	{
		_RampTex("Ramp", 2D) = "white" {}
		_Color("Color", Color) = (0.5, 0.5, 0.5, 1)
		_ColorHorizon("Horizon Color", Color) = (0.5, 0.5, 0.5, 1)
		[NoScaleOffset] _ReflectionTex("Reflection Texture", 2D) = "white" {}
		[Normal] _RippleTex("Ripple Texture", 2D) = "white" {}
		_RippleStrength("Ripple Strength", Float) = 0.5
		_RippleSpeed("Ripple Speed", Float) = 0
		_ReflectionStrength("Reflection Strength", Float) = 0.5
		_FadeDistance("Fade Distance", Float) = 0
		_FadeScaleX("Fade Scale X", Float) = 4
	}
	SubShader
	{
		Tags
		{
			"Queue"="Geometry"
			"RenderType"="Opaque"
		}

		ZWrite Off
		Cull Back

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uvRamp : TEXCOORD0;
				float2 uvRipple : TEXCOORD1;
				float4 screenPos : TEXCOORD2;
				float4 worldPos : TEXCOORD3;
			};

			sampler2D _RampTex;
			float4 _RampTex_ST;

			sampler2D _RippleTex;
			float4 _RippleTex_ST;

			sampler2D _ReflectionTex;
			float4 _ReflectionTex_ST;

			fixed4 _Color;
			fixed4 _ColorHorizon;
			fixed _RippleStrength;
			fixed _RippleSpeed;
			fixed _ReflectionStrength;
			fixed _FadeDistance;
			fixed _FadeScaleX;

			// Globals
			fixed4 _LiminalGroundFogColor;
			fixed _LiminalLightwaveStrength;
			fixed _LiminalLightwaveDistance;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uvRamp = TRANSFORM_TEX(v.uv, _RampTex);
				o.uvRipple = TRANSFORM_TEX(v.uv, _RippleTex);
				
				o.screenPos = ComputeNonStereoScreenPos(o.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 main = _LiminalGroundFogColor;
				fixed4 ramp = tex2D(_RampTex, i.uvRamp);

				// Apply rippling effect via normalmap to the screen position
				float2 uvr = i.uvRipple + float2(0, _Time.x * _RippleSpeed);
				fixed3 nrm = UnpackNormal(tex2D(_RippleTex, uvr));
				i.screenPos.xy += nrm.r * _RippleStrength;

				float2 uvRefl = i.screenPos.xy / i.screenPos.w;
				fixed4 refl = tex2D(_ReflectionTex, uvRefl) * _ReflectionStrength;
				
				fixed2 delta = _WorldSpaceCameraPos.xz - i.worldPos.xz;
				fixed dist = length(fixed2(delta.x * _FadeScaleX, delta.y));
				
				// Fade reflection over XZ distance
				refl *= saturate(pow(dist / _FadeDistance, 4));
				refl  = saturate(refl - Luminance(main) / 6);

				// Combine main color and reflection
				fixed4 col = saturate(main + refl);
				
				// Apply ramp to fade toward horizon color
				col = lerp(col, _ColorHorizon, 1 - ramp);
				
				// Lightwave
				col *= 1 - pow(saturate(length(i.worldPos.xz) / _LiminalLightwaveDistance), 6);

				return col;
			}
			ENDCG
		}
	}
}
