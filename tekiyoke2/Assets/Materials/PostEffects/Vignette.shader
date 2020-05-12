﻿Shader "Unlit/Vignette"
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
                fixed4 col = tex2D(_MainTex, input.uv);
                col -= float4(0.7,0.8,0.95,0) * ( edge(input.uv.x) / 1.5 + edge(input.uv.y) / 3 ) * _Volume;
                return col;
            }
            ENDCG
        }
    }
}
