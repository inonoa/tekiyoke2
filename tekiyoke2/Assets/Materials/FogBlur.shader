Shader "Unlit/FogBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0.7,0.7,0.8,0.5)
        _AlphaRate ("Alpha Rate(for depth)", float) = 1
        _GapMax ("Gap Max", float) = 0.002
        _ResolutionMax ("Resolution Max", int) = 5
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
        }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

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
            float _GapMax;
            int _ResolutionMax;
            fixed4 _Color;
            float _AlphaRate;

            VertToFrag vert (VertInput vert)
            {
                VertToFrag output;

                output.vertex = UnityObjectToClipPos(vert.vertex);
                output.uv     = vert.uv;
                output.color  = vert.color;

                return output;
            }

            float random (fixed2 p){
                return frac(sin(dot(p, fixed2(12.9898,78.233))) * 43758.5453);
            }

            fixed4 frag (VertToFrag input) : SV_Target
            {
                fixed4 col = fixed4(0,0,0,0);

                //縦横二回やるのこれだと不適切感あるな……
                for(int i = -_ResolutionMax; i < 1+_ResolutionMax; i++ ){
                    float2 uv_yoko = input.uv + float2(i, 0) * _GapMax/_ResolutionMax;
                    fixed4 yoko = tex2D(_MainTex, uv_yoko);
                    float rdRange = saturate(0.4-abs(yoko.a - 0.5));
                    yoko.a += random(uv_yoko + _SinTime.xy) * rdRange * 2 - rdRange;
                    yoko.a = saturate(yoko.a);
                    col += yoko;
                }
                for(int j = -_ResolutionMax; j < 1+_ResolutionMax; j++ ){
                    float2 uv_tate = input.uv + float2(0, j) * _GapMax/_ResolutionMax;
                    fixed4 tate = tex2D(_MainTex, uv_tate);
                    float rdRange = saturate(0.4-abs(tate.a - 0.5));
                    tate.a += random(uv_tate + _SinTime.xy) * rdRange * 2 - rdRange;
                    tate.a = saturate(tate.a);
                    col += tate;
                }
                col /= 2 + 4 * _ResolutionMax;
                col.a *= _AlphaRate;
                //col.rgb *= col.a;
                col *= _Color;

                return col;
            }
            ENDCG
        }
    }
}
