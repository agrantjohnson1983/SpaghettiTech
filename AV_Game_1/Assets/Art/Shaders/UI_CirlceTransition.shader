Shader "UI/Circle Transition"
{
    Properties
    {
        _Color ("Color", Color) = (0,0,0,1)

        _Progress ("Progress", Range(0,1)) = 0

        _Softness ("Softness", Range(0,0.25)) = 0.02

        _Invert ("Invert", Float) = 0
    }


    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }


        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha


        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };


            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };


            fixed4 _Color;

            float _Progress;

            float _Softness;

            float _Invert;



            v2f vert(appdata v)
            {
                v2f o;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = v.uv;

                return o;
            }



            fixed4 frag(v2f i) : SV_Target
            {
                //
                // Center UV coordinates
                //
                float2 uv = i.uv - float2(0.5, 0.5);


                //
                // Correct circle for screen aspect ratio
                //
                float aspect =
                    _ScreenParams.x / _ScreenParams.y;


                uv.x *= aspect;


                //
                // Distance from center
                //
                float distance =
                    length(uv);



                //
                // Circle radius
                //
                float radius =
                    _Progress * 0.8;



                //
                // Soft circle edge
                //
                float alpha =
                    smoothstep(
                        radius,
                        radius + _Softness,
                        distance
                    );



                //
                // Reverse direction if needed
                //
                if (_Invert > 0.5)
                {
                    alpha = 1 - alpha;
                }



                return fixed4(
                    _Color.rgb,
                    alpha
                );
            }


            ENDCG
        }
    }
}