Shader "AV/PlacementPreview"
{
    // Drive via MaterialPropertyBlock, same pattern as your highlighter:
    //   block.SetColor("_ValidColor", Color.green);
    //   block.SetColor("_InvalidColor", Color.red);
    //   block.SetFloat("_IsValid", isValid ? 1f : 0f);
    Properties
    {
        _ValidColor ("Valid Color", Color) = (0.2, 1.0, 0.3, 0.35)
        _InvalidColor ("Invalid Color", Color) = (1.0, 0.2, 0.2, 0.35)
        _IsValid ("Is Valid (0/1)", Range(0,1)) = 1
        _FresnelPower ("Fresnel Power", Range(0.1, 8)) = 2.5
        _FresnelStrength ("Fresnel Strength", Range(0, 3)) = 1.5
        _PulseSpeed ("Pulse Speed", Float) = 2.0
        _PulseAmount ("Pulse Amount", Range(0, 1)) = 0.15
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _ValidColor;
                float4 _InvalidColor;
                float _IsValid;
                float _FresnelPower;
                float _FresnelStrength;
                float _PulseSpeed;
                float _PulseAmount;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs vpi = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionHCS = vpi.positionCS;
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.viewDirWS = GetWorldSpaceViewDir(vpi.positionWS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 normalWS = normalize(IN.normalWS);
                float3 viewDirWS = normalize(IN.viewDirWS);

                float fresnel = pow(1.0 - saturate(dot(normalWS, viewDirWS)), _FresnelPower) * _FresnelStrength;

                float pulse = 1.0 + sin(_Time.y * _PulseSpeed) * _PulseAmount;

                half4 baseColor = lerp(_InvalidColor, _ValidColor, _IsValid);
                half3 finalColor = baseColor.rgb * pulse + fresnel * baseColor.rgb;
                half finalAlpha = saturate(baseColor.a + fresnel * 0.4);

                return half4(finalColor, finalAlpha);
            }
            ENDHLSL
        }
    }
}
