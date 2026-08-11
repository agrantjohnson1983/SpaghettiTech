Shader "AV/Dissolve"
{
    // Spawn boxes/truss in, or dissolve them out on gig teardown.
    // Drive via MaterialPropertyBlock, animate _DissolveAmount 0 to 1 over time:
    //   block.SetFloat("_DissolveAmount", t);
    // Needs a noise texture assigned per-material (not MPB-friendly for textures,
    // so set _NoiseTex on the Material asset itself).
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.7, 0.7, 0.7, 1)
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _DissolveAmount ("Dissolve Amount (0-1)", Range(0,1)) = 0
        _EdgeWidth ("Edge Width", Range(0.001, 0.3)) = 0.05
        _EdgeColor ("Edge Color", Color) = (1.0, 0.5, 0.1, 1)
        _EdgeGlow ("Edge Glow Strength", Range(0, 5)) = 2.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" "RenderPipeline"="UniversalPipeline" }
        LOD 100

        Pass
        {
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float2 uv : TEXCOORD1;
                float2 noiseUV : TEXCOORD2;
            };

            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_NoiseTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseColor;
                float4 _NoiseTex_ST;
                float _DissolveAmount;
                float _EdgeWidth;
                float4 _EdgeColor;
                float _EdgeGlow;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = IN.uv;
                OUT.noiseUV = TRANSFORM_TEX(IN.uv, _NoiseTex);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float noise = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, IN.noiseUV).r;

                clip(noise - _DissolveAmount);

                float edgeMask = 1.0 - smoothstep(_DissolveAmount, _DissolveAmount + _EdgeWidth, noise);

                Light mainLight = GetMainLight();
                float ndotl = saturate(dot(normalize(IN.normalWS), mainLight.direction));
                half3 lit = _BaseColor.rgb * (0.3 + 0.7 * ndotl) * mainLight.color;

                half3 finalColor = lit + _EdgeColor.rgb * edgeMask * _EdgeGlow;

                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}
