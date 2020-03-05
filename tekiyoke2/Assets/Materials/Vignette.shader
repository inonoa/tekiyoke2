Shader "Unlit/Vignette"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Volume ("Volume", float) = 0.3
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
            float _Volume;

            VertToFrag vert (VertInput vert)
            {
                VertToFrag output;

                output.vertex = UnityObjectToClipPos(vert.vertex);
                output.uv     = vert.uv;
                output.color  = vert.color;

                return output;
            }

            float edge(float x){
                return 2 * x * x  + 2 * (1-x) * (1-x) - 1;
            }

            fixed4 frag (VertToFrag input) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, input.uv);
                col -= float4(0.7,0.8,1,0) * edge(input.uv.x) * edge(input.uv.y) * _Volume;
                return col;
            }
            ENDCG
        }
    }
}
