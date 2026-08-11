Shader "AV/FresnelRimGeneric"
{
    // General-purpose rim/fresnel glow, not tied to any one feature. Use it
    // for selection emphasis, tool-in-hand glow, "interactable" hinting,
    // anything that just needs an edge light. More configurable than
    // PlacementPreview since it keeps your base lit color instead of
    // replacing it.
    Properties
    {
        _BaseMap ("Base Map", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _RimColor ("Rim Color", Color) = (0.3, 0.8, 1.0, 1)
        _RimPower ("Rim Power", Range(0.1, 8)) = 3.0
        _RimStrength ("Rim Strength", Range(0, 5)) = 1.5
        _RimPulseSpeed ("Rim Pulse Speed (0 = static)", Float) = 0
        _RimPulseAmount ("Rim Pulse Amount", Range(0,1)) = 0.3
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
                float3 viewDirWS : TEXCOORD1;
                float2 uv : TEXCOORD2;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float4 _RimColor;
                float _RimPower;
                float _RimStrength;
                float _RimPulseSpeed;
                float _RimPulseAmount;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs vpi = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionHCS = vpi.positionCS;
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.viewDirWS = GetWorldSpaceViewDir(vpi.positionWS);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);

                float3 normalWS = normalize(IN.normalWS);
                float3 viewDirWS = normalize(IN.viewDirWS);

                Light mainLight = GetMainLight();
                float ndotl = saturate(dot(normalWS, mainLight.direction));
                half3 lit = tex.rgb * _BaseColor.rgb * (0.3 + 0.7 * ndotl) * mainLight.color;

                float fresnel = pow(1.0 - saturate(dot(normalWS, viewDirWS)), _RimPower);
                float pulse = _RimPulseSpeed > 0.0
                    ? 1.0 + sin(_Time.y * _RimPulseSpeed) * _RimPulseAmount
                    : 1.0;

                half3 finalColor = lit + fresnel * _RimStrength * pulse * _RimColor.rgb;

                return half4(finalColor, tex.a * _BaseColor.a);
            }
            ENDHLSL
        }
    }
}
