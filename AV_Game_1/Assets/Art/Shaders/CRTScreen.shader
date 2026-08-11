Shader "AV/CRTScreen"
{
    // Diegetic screen look for mixing console displays, amp readouts, etc.
    // Feed it a real render texture or a static UI texture in _ScreenTex.
    Properties
    {
        _ScreenTex ("Screen Texture", 2D) = "black" {}
        _TintColor ("Tint Color", Color) = (0.6, 1.0, 0.7, 1)
        _ScanlineCount ("Scanline Count", Float) = 120
        _ScanlineStrength ("Scanline Strength", Range(0,1)) = 0.3
        _CurveAmount ("Screen Curve Amount", Range(0, 0.5)) = 0.08
        _Brightness ("Brightness", Range(0.5, 3)) = 1.3
        _FlickerSpeed ("Flicker Speed", Float) = 8
        _FlickerStrength ("Flicker Strength", Range(0,0.3)) = 0.03
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" "RenderPipeline"="UniversalPipeline" }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_ScreenTex);
            SAMPLER(sampler_ScreenTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _ScreenTex_ST;
                float4 _TintColor;
                float _ScanlineCount;
                float _ScanlineStrength;
                float _CurveAmount;
                float _Brightness;
                float _FlickerSpeed;
                float _FlickerStrength;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _ScreenTex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Barrel-curve the UVs slightly for a CRT feel
                float2 centered = IN.uv * 2.0 - 1.0;
                float2 curved = centered * (1.0 + _CurveAmount * dot(centered, centered));
                float2 uv = curved * 0.5 + 0.5;

                // Outside curved bounds -> black bezel
                float inBounds = step(0.0, uv.x) * step(uv.x, 1.0) * step(0.0, uv.y) * step(uv.y, 1.0);

                half4 tex = SAMPLE_TEXTURE2D(_ScreenTex, sampler_ScreenTex, uv);

                float scanline = sin(uv.y * _ScanlineCount * PI * 2.0) * 0.5 + 0.5;
                float scanlineMul = lerp(1.0, scanline, _ScanlineStrength);

                float flicker = 1.0 + (sin(_Time.y * _FlickerSpeed) * 0.5) * _FlickerStrength;

                half3 finalColor = tex.rgb * _TintColor.rgb * scanlineMul * _Brightness * flicker;
                finalColor *= inBounds;

                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}
