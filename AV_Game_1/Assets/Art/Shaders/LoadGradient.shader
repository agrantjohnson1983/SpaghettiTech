Shader "AV/LoadGradient"
{
    // Reads vertex color R channel as a 0-1 load value and maps it through
    // a 3-stop color gradient (safe -> warning -> overloaded). Useful for
    // sCableSegment tension or truss load display without any texture work -
    // just write load into vertex.color.r from your connection/load system.
    Properties
    {
        _SafeColor ("Safe Color", Color) = (0.2, 0.9, 0.3, 1)
        _WarningColor ("Warning Color", Color) = (1.0, 0.8, 0.0, 1)
        _OverloadColor ("Overload Color", Color) = (1.0, 0.1, 0.1, 1)
        _WarningThreshold ("Warning Threshold", Range(0,1)) = 0.6
        _OverloadThreshold ("Overload Threshold", Range(0,1)) = 0.9
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
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float loadValue : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _SafeColor;
                float4 _WarningColor;
                float4 _OverloadColor;
                float _WarningThreshold;
                float _OverloadThreshold;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.loadValue = IN.color.r;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                half3 gradColor;
                if (IN.loadValue < _WarningThreshold)
                {
                    float t = IN.loadValue / max(_WarningThreshold, 0.0001);
                    gradColor = lerp(_SafeColor.rgb, _WarningColor.rgb, t);
                }
                else
                {
                    float t = (IN.loadValue - _WarningThreshold) / max(_OverloadThreshold - _WarningThreshold, 0.0001);
                    gradColor = lerp(_WarningColor.rgb, _OverloadColor.rgb, saturate(t));
                }

                Light mainLight = GetMainLight();
                float ndotl = saturate(dot(normalize(IN.normalWS), mainLight.direction));
                half3 finalColor = gradColor * (0.35 + 0.65 * ndotl) * mainLight.color;

                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}
