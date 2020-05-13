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
                fixed4 col = tex2D(_MainTex, input.uv);
                col *= _Color;
                col.a += (random(input.uv + _Time.xy) - 0.5) * (0.3 - abs(col.a - 0.5));

                return col;
            }
            ENDCG
        }
    }
}
