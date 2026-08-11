Shader "AV/RadialProgress"
{
    // Drive via MaterialPropertyBlock:
    //   block.SetFloat("_Progress", torqueAmount01);
    //   block.SetColor("_FillColor", inProgressColor);
    //   block.SetColor("_CompleteColor", doneColor);
    // Good on a flat quad/disc facing camera for a bolt-tightening gauge,
    // or reused anywhere you need a generic "hold to complete" ring.
    Properties
    {
        _BackgroundColor ("Background Color", Color) = (0.1, 0.1, 0.1, 0.6)
        _FillColor ("Fill Color", Color) = (1.0, 0.7, 0.1, 1.0)
        _CompleteColor ("Complete Color", Color) = (0.2, 1.0, 0.3, 1.0)
        _Progress ("Progress (0-1)", Range(0,1)) = 0
        _RingThickness ("Ring Thickness", Range(0.01, 0.5)) = 0.15
        _StartAngle ("Start Angle (deg)", Range(0, 360)) = 90
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

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
                float4 _BackgroundColor;
                float4 _FillColor;
                float4 _CompleteColor;
                float _Progress;
                float _RingThickness;
                float _StartAngle;
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
                float2 centered = IN.uv - 0.5;
                float dist = length(centered) * 2.0;

                // Ring mask
                float outerEdge = 1.0;
                float innerEdge = 1.0 - _RingThickness;
                float ringMask = smoothstep(outerEdge, outerEdge - 0.02, dist) *
                                  smoothstep(innerEdge - 0.02, innerEdge, dist);

                // Angle of this pixel, offset by start angle, going clockwise
                float angle = atan2(centered.y, centered.x);
                angle = degrees(angle);
                angle = fmod(angle - _StartAngle + 360.0 + 90.0, 360.0);
                float normalizedAngle = angle / 360.0;

                float progressMask = step(normalizedAngle, _Progress);

                half4 fillCol = lerp(_FillColor, _CompleteColor, step(0.999, _Progress));
                half4 finalColor = lerp(_BackgroundColor, fillCol, progressMask);
                finalColor.a *= ringMask;

                return finalColor;
            }
            ENDHLSL
        }
    }
}
