Shader "Hidden/Draft"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _EdgeColor ("Edge Color", Color) = (0, 0.8, 1, 1)
        _d_uv ("Delta UV", Range(0, 0.005)) = 0.003
        _BaseColorRate ("Base Color Rate", Range(0, 1)) = 0.2
        [Toggle] _ReversesBaseColor ("Reverses Base Color", Int) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // プラットフォームによってはUVの上下が逆になるので補正をいれる
                #if UNITY_UV_STARTS_AT_TOP 
                o.uv = float2(v.uv.x, 1.0 - v.uv.y); 
                #else
                o.uv = v.uv;
                #endif

                return o;
            }

            sampler2D _MainTex;
            fixed4 _EdgeColor;
            float _d_uv;
            float _BaseColorRate;
            bool _ReversesBaseColor;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                if(_ReversesBaseColor)
                {
                    col = fixed4(1, 1, 1, 1) - col;
                    col.a = 1;
                }

                fixed4 colur = tex2D(_MainTex, i.uv + fixed2( _d_uv,  _d_uv));
                fixed4 colul = tex2D(_MainTex, i.uv + fixed2(-_d_uv,  _d_uv));
                fixed4 coldr = tex2D(_MainTex, i.uv + fixed2( _d_uv, -_d_uv));
                fixed4 coldl = tex2D(_MainTex, i.uv + fixed2(-_d_uv, -_d_uv));

                float d_col_1 = length(coldl - colur);
                float d_col_2 = length(colul - coldr);

                fixed4 edgeCol = _EdgeColor * (d_col_1 + d_col_2);

                return col * _BaseColorRate + edgeCol;
            }
            ENDCG
        }
    }
}
