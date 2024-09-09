Shader "Custom/SkyboxShader_New" {
    Properties{
        _Skybox1("Skybox 1 (HDR)", CUBE) = "" {}
        _Exposure1("Exposure 1", Range(0, 8)) = 1.0
        _Tint1("Tint Color 1", Color) = (.5, .5, .5, .5)
        _Rotation1("Rotation 1", Range(0, 360)) = 0

        _Skybox2("Skybox 2 (HDR)", CUBE) = "" {}
        _Exposure2("Exposure 2", Range(0, 8)) = 1.0
        _Tint2("Tint Color 2", Color) = (.5, .5, .5, .5)
        _Rotation2("Rotation 2", Range(0, 360)) = 0

        _CycleDuration("Cycle Duration (seconds)", Range(1, 3600)) = 300
    }

        SubShader{
            Tags {
                "Queue" = "Background"
                "RenderType" = "Background"
                "PreviewType" = "Skybox"
            }
            Cull Off ZWrite Off

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 3.0

                #include "UnityCG.cginc"

                samplerCUBE _Tex1;
                half4 _Tint1;
                half _Exposure1;
                float _Rotation1;

                samplerCUBE _Tex2;
                half4 _Tint2;
                half _Exposure2;
                float _Rotation2;

                float _CycleDuration;

                struct appdata_t {
                    float4 vertex : POSITION;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct v2f {
                    float4 vertex : SV_POSITION;
                    float3 texcoord : TEXCOORD0;
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                v2f vert(appdata_t v)
                {
                    v2f o;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                    half t = frac(_Time.y / _CycleDuration);

                    // Interpolate between the properties of the two skyboxes
                    o.texcoord = lerp(v.vertex.xyz, v.vertex.xyz, t);

                    o.vertex = UnityObjectToClipPos(o.texcoord);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    half4 tex1 = texCUBE(_Tex1, i.texcoord);
                    half4 tex2 = texCUBE(_Tex2, i.texcoord);

                    // Apply exposure and tint
                    tex1.rgb *= _Tint1.rgb * unity_ColorSpaceDouble.rgb;
                    tex2.rgb *= _Tint2.rgb * unity_ColorSpaceDouble.rgb;
                    tex1.rgb *= _Exposure1;
                    tex2.rgb *= _Exposure2;

                    half3 blendedColor = lerp(tex1.rgb, tex2.rgb, i.texcoord);

                    return half4(blendedColor, 1);
                }
                ENDCG
            }
        }

            Fallback Off
}