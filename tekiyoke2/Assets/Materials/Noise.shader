Shader "Unlit/DefaultShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            VertToFrag vert (VertInput vert)
            {
                VertToFrag output;

                output.vertex = UnityObjectToClipPos(vert.vertex);
                output.uv     = vert.uv;
                output.color  = vert.color;

                return output;
            }

            fixed4 frag (VertToFrag input) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, input.uv);
                
                return col;
            }
            ENDCG
        }
    }
}
