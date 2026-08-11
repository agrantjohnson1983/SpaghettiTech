Shader "AV/HazardPulse"
{
    // For flagging an overloaded truss point, unsafe rigging, or an object
    // that needs attention. Drive via MaterialPropertyBlock:
    //   block.SetFloat("_HazardLevel", loadPercent01); // scales stripe speed + pulse
    Properties
    {
        _StripeColorA ("Stripe Color A", Color) = (1.0, 0.8, 0.0, 1)
        _StripeColorB ("Stripe Color B", Color) = (0.1, 0.1, 0.1, 1)
        _StripeScale ("Stripe Scale", Float) = 8.0
        _StripeAngle ("Stripe Angle (deg)", Range(0, 90)) = 45
        _ScrollSpeed ("Scroll Speed", Float) = 1.0
        _HazardLevel ("Hazard Level (0-1)", Range(0,1)) = 1.0
        _PulseSpeed ("Pulse Speed", Float) = 4.0
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
                float4 _StripeColorA;
                float4 _StripeColorB;
                float _StripeScale;
                float _StripeAngle;
                float _ScrollSpeed;
                float _HazardLevel;
                float _PulseSpeed;
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
                float rad = radians(_StripeAngle);
                float2 dir = float2(cos(rad), sin(rad));

                float scroll = _Time.y * _ScrollSpeed * max(_HazardLevel, 0.05);
                float coord = dot(IN.uv, dir) * _StripeScale + scroll;

                float stripe = frac(coord) < 0.5 ? 1.0 : 0.0;
                half4 stripeColor = lerp(_StripeColorB, _StripeColorA, stripe);

                float pulse = 0.7 + 0.3 * sin(_Time.y * _PulseSpeed * max(_HazardLevel, 0.05));
                stripeColor.rgb *= lerp(1.0, pulse, _HazardLevel);

                return stripeColor;
            }
            ENDHLSL
        }
    }
}
