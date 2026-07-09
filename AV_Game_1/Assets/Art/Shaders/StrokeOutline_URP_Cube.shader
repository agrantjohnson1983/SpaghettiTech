Shader "Custom/StrokeOutline_URP"
{
    Properties
    {
        [Header(Base Surface)]
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _BaseMap ("Base Map", 2D) = "white" {}

        [Header(Outline)]
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range(0.0, 0.2)) = 0.02
        _OutlineWidthMode ("Width Mode (0=Object Space, 1=Screen Space)", Range(0,1)) = 0

        [Header(Face Edges)]
        _EdgeColor ("Edge Color", Color) = (0,0,0,1)
        _EdgeThickness ("Edge Thickness (pixels)", Range(0.5, 10)) = 1.5
        [Toggle] _ShowFaceEdges ("Show Face Edges", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" }

        // ---------- PASS 1: Outline (inverted hull) ----------
        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "SRPDefaultUnlit" }

            Cull Front         // draw only back faces
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _OutlineColor;
                float _OutlineWidth;
                float _OutlineWidthMode;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;

                // Object-space normal extrusion — cheap and stable on hard-edged
                // geometry like cubes, since each face normal is constant per-vertex
                // (assuming the cube's vertices are NOT smoothed/shared across faces).
                float3 positionOS = IN.positionOS.xyz + IN.normalOS * _OutlineWidth;

                if (_OutlineWidthMode > 0.5)
                {
                    // Screen-space width: keeps outline thickness constant regardless
                    // of distance from camera. Extrude in clip space instead.
                    float4 posClip = TransformObjectToHClip(IN.positionOS.xyz);
                    float3 normalWS = TransformObjectToWorldNormal(IN.normalOS);
                    float3 normalVS = TransformWorldToViewDir(normalWS, true);
                    // Push along view-space normal scaled by clip-space W (distance)
                    posClip.xy += normalize(normalVS.xy + 1e-5) * _OutlineWidth * posClip.w * 0.1;
                    OUT.positionHCS = posClip;
                }
                else
                {
                    OUT.positionHCS = TransformObjectToHClip(positionOS);
                }

                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }

        // ---------- PASS 2: Base lit surface ----------
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Cull Back
            ZWrite On
            ZTest LEqual

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
                half4 _EdgeColor;
                float _EdgeThickness;
                float _ShowFaceEdges;
            CBUFFER_END

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS    : TEXCOORD0;
                float3 positionWS  : TEXCOORD1;
                float2 uv          : TEXCOORD2;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionHCS = TransformWorldToHClip(OUT.positionWS);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.uv = IN.uv; // raw 0..1 mesh UV — used for edge detection, NOT tiled
                return OUT;
            }

            // Returns 0..1 mask, 1 = on a UV border of the current face.
            // Uses fwidth() (screen-space UV derivative) so the line stays a
            // constant pixel width regardless of distance, angle, or face size.
            half FaceEdgeMask(float2 uv, float thicknessPixels)
            {
                float2 d = fwidth(uv) * thicknessPixels;
                float2 distToEdge = min(uv, 1.0 - uv);       // distance to nearest UV border (0 or 1)
                float2 lineMask = 1.0 - smoothstep(0.0, d, distToEdge);
                return saturate(max(lineMask.x, lineMask.y));
            }

            half4 frag (Varyings IN) : SV_Target
            {
                Light mainLight = GetMainLight();
                half3 normal = normalize(IN.normalWS);

                half NdotL = saturate(dot(normal, mainLight.direction));
                half3 lighting = mainLight.color * NdotL + SampleSH(normal) * 0.5;

                half4 texColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv * _BaseMap_ST.xy + _BaseMap_ST.zw);
                half4 col = texColor * _BaseColor;
                col.rgb *= lighting;

                if (_ShowFaceEdges > 0.5)
                {
                    half edge = FaceEdgeMask(IN.uv, _EdgeThickness);
                    // --- TEMP DEBUG: uncomment the next line to see the raw mask ---
                    // return half4(edge, edge, edge, 1);
                    col.rgb = lerp(col.rgb, _EdgeColor.rgb, edge * _EdgeColor.a);
                }

                return col;
            }
            ENDHLSL
        }
    }

    FallBack "Universal Render Pipeline/Lit"
}
