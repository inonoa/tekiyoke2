Shader "Unlit/WhiteOut"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Whiteness ("Whiteness", Range(0, 1)) = 0
        _White ("White", Color) = (1, 1, 1, 1)
        _Black ("Black", Color) = (0, 0, 0, 1)
        _Alpha ("Alpha", Range(0, 1)) = 1
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Whiteness;
            float4 _White;
            float4 _Black;
            float _Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float whiteness_pixel = 1 - tex2D(_MainTex, i.uv).r;

                if(whiteness_pixel < _Whiteness) return _White * float4(1, 1, 1, _Alpha);
                if(whiteness_pixel < _Whiteness * 1.05) return _Black * float4(1, 1, 1, _Alpha);
                return fixed4(0, 0, 0, 0);
            }
            ENDCG
        }
    }
}
