Shader "Unlit/Hero"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}
        _DisThreshold0 ("Dissolve Threshold 0", float) = 0
        _DisThreshold1 ("Dissolve Threshold 1", float) = 1
        _Alpha ("Alpha", float) = 1
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
                float4 bgPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            sampler2D _DissolveTex;
            float _DisThreshold0;
            float _DisThreshold1;
            float _Alpha;

            VertToFrag vert (VertInput vert)
            {
                VertToFrag output;

                output.vertex = UnityObjectToClipPos(vert.vertex);
                output.uv     = vert.uv;
                output.bgPos  = ComputeGrabScreenPos(output.vertex);
                output.color  = vert.color;

                return output;
            }

            float random (fixed2 p){
                return frac(sin(dot(p, fixed2(12.9898,78.233))) * 43758.5453);
            }

            float hashi(float v){
                return v > 0.5 ? 3 - 2 * v : 2 * v;
            }

            fixed4 frag (VertToFrag input) : SV_Target
            {
                fixed4 mainCol = tex2D(_MainTex, input.uv);
                fixed4 col = mainCol;

                fixed4 dissolveCol = tex2D(_DissolveTex, input.uv);
                float dissolveAlpha = saturate((dissolveCol.r - _DisThreshold0) / (_DisThreshold1 - _DisThreshold0));
                col.a *= dissolveAlpha * _Alpha;

                return col;
            }
            ENDCG
        }
    }
}
