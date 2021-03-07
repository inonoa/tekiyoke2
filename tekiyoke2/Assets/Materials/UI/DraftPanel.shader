Shader "Unlit/DraftPanel"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Contrast ("Contrast", Range(0, 1)) = 0
        _Light ("Light", Range(0, 3)) = 0
        _LightTex ("Light Texture", 2D) = "black" {}
        _LightAreaThreshold("Lit Area Threshold", Range(-0.1, 1.1)) = 0
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            sampler2D _LightTex;
            float4 _MainTex_ST;
            float _Contrast;
            float _Light;
            float _LightAreaThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            float contrasted(float v)
            {
                return _Contrast * (v * v * v) + (1 - _Contrast) * v;
            }

            float easeAroundThreshold(float v)
            {
                return (v > 0.5) ? (1 - 2 * (1 - v) * (1 - v)) : (2 * v * v);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 baseCol = tex2D(_MainTex, i.uv);
                float4 contrastAdded = float4(contrasted(baseCol.r), contrasted(baseCol.g), contrasted(baseCol.b), baseCol.a) * i.color;
                float lightX = (i.uv.x + (1 - i.uv.y) / 16.0) * 16 / 17.0;
                float litness = lightX < _LightAreaThreshold ?
                    1 + easeAroundThreshold(50 * saturate(0.02 + lightX - _LightAreaThreshold)):
                        easeAroundThreshold(50 * saturate(0.02 + _LightAreaThreshold - lightX)) * 2;
                fixed4 lit = contrastAdded + saturate(float4((tex2D(_LightTex, i.uv).rgb - float3(0.05, 0.05, 0.05)) / float3(0.95, 0.95, 0.95), 0)) * _Light * litness;

                return lit;
            }
            ENDCG
        }
    }
}
