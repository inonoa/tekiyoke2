Shader "Unlit/Noise"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Volume ("Volume", float) = 0.2
        _Density("Density", float) = 0.025
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
            float _Volume;
            float _Density;

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

            float random (fixed2 p){
                return frac(sin(dot(p, fixed2(12.9898,78.233))) * 43758.5453);
            }

            float odoriba(float x){
                return (x <= 1 - _Density) ? (x < _Density) ? (x / _Density / 2) : 0.5 : ( (x-1) / _Density / 2 + 1);
            }

            fixed4 frag (VertToFrag input) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, input.uv);
                float rd = random( floor(input.uv * 100) + _Time * 7 % 100);
                col -= fixed4(1,1,1,0) * _Volume * (odoriba(rd) - 0.5);

                return col;
            }
            ENDCG
        }
    }
}
