Shader "Unlit/BlurShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BlurTex("Texture", 2D) = "white" {}
        _BlurSize("Blur Size", Range(0,.1)) = 0
        _BlurPower("Blur Power", Range(0,1)) = 0
        _Height("Blur Power", float) = 1080
        _Width("Blur Power", float) = 1920
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            #define SAMPLES 10

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _BlurTex;
            float4 _BlurTex_ST;
            float _BlurSize;
            float _BlurPower;
            float _Height;
            float _Width;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //init color variable
                float4 mainCol = tex2D(_MainTex, i.uv);
                float4 col = 0;
                float aspectRatio = _Width / _Height;

                float toRadians = 3.14159265 / 180;

                //iterate over blur samples
                //for (float indexx = 0; indexx < 25; indexx++)
                //{
                //    for (float indexy = 0; indexy < 25; indexy++)
                //    {
                //            //get uv coordinate of sample
                //            float2 uv = i.uv + float2((indexx / (25 - 1) - 0.5) * (_BlurSize), (indexy / (25 - 1) - 0.5) * (_BlurSize * aspectRatio));
                //            //add color at position to color
                //            col += tex2D(_BlurTex, uv);
                //    }
                //}
                
                for (float d=0; d < 360; d+=10)
                {
                    float2 uv = float2(cos(d * toRadians), sin(d * toRadians) * aspectRatio) * _BlurSize;

                    for (float samp = 0; samp < SAMPLES; ++samp)
                    {
                        col += tex2D(_BlurTex, i.uv + uv*(samp/(SAMPLES-1)));
                    }
                }

                //divide the sum of values by the amount of samples
                col = col / SAMPLES;

                mainCol += (col * _BlurPower);
                return mainCol;
            }
            ENDCG
        }
    }
}
