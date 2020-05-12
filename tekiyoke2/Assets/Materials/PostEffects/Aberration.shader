Shader "Unlit/Aberration"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Gap ("Gap", float) = 0.002
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct VertInput
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct VertToFrag
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float _Gap;

            VertToFrag vert (VertInput vert)
            {
                VertToFrag output;

                output.vertex = UnityObjectToClipPos(vert.vertex);

                // プラットフォームによってはUVの上下が逆になるので補正をいれる
                #if UNITY_UV_STARTS_AT_TOP 
                output.uv = float2(vert.uv.x, 1.0 - vert.uv.y); 
                #else
                output.uv = vert.uv;
                #endif
                
                output.color  = vert.color;

                return output;
            }

            float outside(float x){
                return x * (1 + _Gap) - 0.5 * _Gap;
            }
            float inside(float x){
                return x * (1 - _Gap) + 0.5 * _Gap;
            }

            float2 outside2(float2 xy){
                float2 f2 = float2(saturate(outside(xy.x)), saturate(outside(xy.y)));
                return f2;
            }
            float2 inside2(float2 xy){
                float2 f2 = float2(inside(xy.x), inside(xy.y));
                return f2;
            }

            fixed4 frag (VertToFrag input) : SV_Target
            {
                fixed4 col = fixed4(1,1,1,1);
                col.r = tex2D(_MainTex, outside2(input.uv)).r;
                col.g = tex2D(_MainTex, input.uv).g;
                col.b = tex2D(_MainTex, inside2(input.uv)).b;

                return col;
            }
            ENDCG
        }
    }
}
