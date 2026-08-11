Shader "AV/Triplanar"
{
    // General-purpose utility: projects a texture from three axes and blends
    // by surface normal, so it looks correct on procedurally generated or
    // unwrapped-badly meshes (crates, cut truss pieces, blockout geometry)
    // with zero UV work.
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Tint ("Tint", Color) = (1,1,1,1)
        _TexScale ("Texture Scale", Float) = 1.0
        _BlendSharpness ("Blend Sharpness", Range(1, 16)) = 4.0
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
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _Tint;
                float _TexScale;
                float _BlendSharpness;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs vpi = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionHCS = vpi.positionCS;
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.positionWS = vpi.positionWS;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 normalWS = normalize(IN.normalWS);
                float3 blendWeights = pow(abs(normalWS), _BlendSharpness);
                blendWeights /= (blendWeights.x + blendWeights.y + blendWeights.z);

                float2 uvX = IN.positionWS.zy * _TexScale;
                float2 uvY = IN.positionWS.xz * _TexScale;
                float2 uvZ = IN.positionWS.xy * _TexScale;

                half4 texX = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uvX);
                half4 texY = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uvY);
                half4 texZ = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uvZ);

                half4 texColor = texX * blendWeights.x + texY * blendWeights.y + texZ * blendWeights.z;

                Light mainLight = GetMainLight();
                float ndotl = saturate(dot(normalWS, mainLight.direction));
                half3 finalColor = texColor.rgb * _Tint.rgb * (0.3 + 0.7 * ndotl) * mainLight.color;

                return half4(finalColor, texColor.a * _Tint.a);
            }
            ENDHLSL
        }
    }
}
