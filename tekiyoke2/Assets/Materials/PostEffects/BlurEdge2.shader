Shader "Unlit/BlurEdge2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GapMax ("Gap Max", float) = 0.002
        _ResolutionMax ("Resolution Max", int) = 5
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
            float _GapMax;
            int _ResolutionMax;

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

            float edge(float x){
                return (x * x * x * x  +  (1-x) * (1-x) * (1-x) * (1-x)) * 8/7 - 1/7;
            }

            fixed4 frag (VertToFrag input) : SV_Target
            {
                fixed4 col = fixed4(0,0,0,0);

                for(int i = -_ResolutionMax; i < 1+_ResolutionMax; i++ ){
                    col += tex2D(_MainTex, input.uv + float2(0, i) * _GapMax/_ResolutionMax * edge(input.uv.x)) * (_ResolutionMax - abs(i) + 1);
                }
                col /= (_ResolutionMax+1)*(_ResolutionMax+1);

                return col;
            }
            ENDCG
        }
    }
}
