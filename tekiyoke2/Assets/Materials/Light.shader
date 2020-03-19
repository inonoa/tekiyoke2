Shader "Unlit/Bane"
{
    Properties
    {
        _MainTex ("Base", 2D) = "white" {}
        _LightColor ("Light Color", Color) = (0.2,0,1, 1)
        _Volume ("Volume", float) = 0.1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
        }
        LOD 100

        ZWrite Off
        Blend SrcAlpha One

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct InputStruct
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct OutputStruct
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float _Volume;
            fixed4 _LightColor;

            OutputStruct vert (InputStruct input)
            {
                OutputStruct output;

                output.vertex = UnityObjectToClipPos(input.vertex);
                output.uv = input.uv;
                output.color = input.color;

                return output;
            }

            fixed4 frag (OutputStruct output) : SV_Target
            {
                float dist = abs(output.uv.x - 0.5) + abs(output.uv.y - 0.5);
                float actVol = (_Volume - 2 * dist / _Volume) * 4/3;

                float colorVol = saturate(actVol);
                float whiteVol = saturate((actVol - 1)*2);

                return _LightColor * colorVol + (fixed4(1,1,1,1) - _LightColor) * whiteVol;
            }
            ENDCG
        }

    }
}
