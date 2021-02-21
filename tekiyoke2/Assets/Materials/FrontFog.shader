Shader "Unlit/FrontFog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OffsetX ("Offset X", Float) = 0
        _Color ("Color", Color) = (1, 1, 1, 1)
        _Alpha0uvX("aが0になるようなuv座標のx(?)", Range(-2, 3)) = -0.1
        _Alpha1uvX("aが1になるようなuv座標のx(?)", Range(-2, 3)) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }
        LOD 100
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _Color;
            float _OffsetX;
            float _Alpha0uvX;
            float _Alpha1uvX;

            float easeInOutSine(float x)
            {
                return saturate(0.5 - 0.5 * cos(saturate(x) * UNITY_PI));
            }
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed a = tex2D(_MainTex, (i.uv + fixed2(_OffsetX, 0)) % fixed2(1, 1)).r;
                fixed a_in_gradation = easeInOutSine((i.uv.x - _Alpha0uvX) / (_Alpha1uvX - _Alpha0uvX));
                return _Color * fixed4(1, 1, 1, a * a_in_gradation);
            }
            ENDCG
        }
    }
}