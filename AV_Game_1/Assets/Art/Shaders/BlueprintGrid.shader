Shader "AV/BlueprintGrid"
{
    // Transparent grid overlay for placement guides on the stage floor or
    // truss snap planes. Works on a flat plane; _GridSpacing is in world
    // units via object scale, or tune _GridScale directly against UVs.
    Properties
    {
        _GridColor ("Grid Line Color", Color) = (0.3, 0.8, 1.0, 0.6)
        _BackgroundColor ("Background Color", Color) = (0.1, 0.3, 0.5, 0.05)
        _GridScale ("Grid Scale (lines per UV)", Float) = 10
        _LineThickness ("Line Thickness", Range(0.001, 0.2)) = 0.02
        _MajorLineEvery ("Major Line Every N", Float) = 5
        _MajorLineThickness ("Major Line Thickness", Range(0.001, 0.3)) = 0.04
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
                float4 _GridColor;
                float4 _BackgroundColor;
                float _GridScale;
                float _LineThickness;
                float _MajorLineEvery;
                float _MajorLineThickness;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            float gridLineMask(float coord, float thickness)
            {
                float cell = frac(coord);
                float distToLine = min(cell, 1.0 - cell);
                return smoothstep(thickness, 0.0, distToLine);
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 gridUV = IN.uv * _GridScale;

                float lineX = gridLineMask(gridUV.x, _LineThickness);
                float lineY = gridLineMask(gridUV.y, _LineThickness);
                float minorMask = saturate(lineX + lineY);

                float majorCoordX = gridUV.x / _MajorLineEvery;
                float majorCoordY = gridUV.y / _MajorLineEvery;
                float majorX = gridLineMask(majorCoordX, _MajorLineThickness / _MajorLineEvery);
                float majorY = gridLineMask(majorCoordY, _MajorLineThickness / _MajorLineEvery);
                float majorMask = saturate(majorX + majorY);

                float finalMask = saturate(minorMask * 0.5 + majorMask);

                half4 finalColor = lerp(_BackgroundColor, _GridColor, finalMask);
                return finalColor;
            }
            ENDHLSL
        }
    }
}
