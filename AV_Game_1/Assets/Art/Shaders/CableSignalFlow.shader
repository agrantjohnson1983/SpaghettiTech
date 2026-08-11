Shader "AV/CableSignalFlow"
{
    // Meant for your sCableSegment mesh, UV.x running along cable length.
    // Drive via MaterialPropertyBlock:
    //   block.SetFloat("_FlowSpeed", isLive ? 1.5f : 0f);
    //   block.SetColor("_SignalColor", signalTypeColor); // e.g. audio=blue, power=orange
    Properties
    {
        _CableColor ("Cable Base Color", Color) = (0.05, 0.05, 0.05, 1)
        _SignalColor ("Signal Color", Color) = (0.2, 0.6, 1.0, 1)
        _FlowSpeed ("Flow Speed", Float) = 1.0
        _DashLength ("Dash Length", Range(0.01, 1)) = 0.15
        _DashGap ("Dash Gap", Range(0.01, 1)) = 0.3
        _GlowStrength ("Glow Strength", Range(0, 5)) = 1.5
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

            CBUFFER_START(UnityPerMaterial)
                float4 _CableColor;
                float4 _SignalColor;
                float _FlowSpeed;
                float _DashLength;
                float _DashGap;
                float _GlowStrength;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float period = _DashLength + _DashGap;
                float scrollU = IN.uv.x - _Time.y * _FlowSpeed;
                float localPos = fmod(scrollU, period);
                localPos = localPos < 0 ? localPos + period : localPos;

                float dashMask = step(localPos, _DashLength);

                // soft edge on the dash for a less harsh look
                float edgeSoft = 0.03;
                float softMask = smoothstep(_DashLength, _DashLength - edgeSoft, localPos) *
                                  smoothstep(0.0, edgeSoft, localPos);
                dashMask = max(dashMask * 0.0, softMask); // keep only soft version

                half3 finalColor = _CableColor.rgb + _SignalColor.rgb * dashMask * _GlowStrength;
                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
}
