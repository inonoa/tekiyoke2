Shader "Unlit/BGFog"
{
    Properties
    {
        _MainTex ("BG Texture", 2D) = "white" {}
        _Fog1 ("Fog Texture 1", 2D) = "white" {}
        _Fog2 ("Fog Texture 2", 2D) = "white" {}
        _Fog3 ("Fog Texture 3", 2D) = "white" {}
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
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
            sampler2D _Fog1;
            sampler2D _Fog2;
            sampler2D _Fog3;

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

                return col;
            }
            ENDCG
        }
    }
}
