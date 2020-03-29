Shader "Unlit/TimeText"
{
    Properties
    {
        _MainTex ("Base", 2D) = "white" {}
        _Radius ("Radius", float) = 1
        _CenterX ("Center X", float) = 0
        _ImageWidth ("ImageWidth", float) = 1
        _ImageHeight ("ImageHeight", float) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 100

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

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
            float _Radius;
            float _CenterX;
            float _ImageWidth;
            float _ImageHeight;

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
                float actualX = _CenterX + ((output.uv.x % 0.125) - 0.0625) * 8 * _ImageWidth;

                float down = _Radius - sqrt( _Radius * _Radius - actualX * actualX );

                float2 newUV = output.uv + float2(0, down) / _ImageHeight;
                newUV.y = saturate(newUV.y);

                return tex2D(_MainTex, newUV);
            }
            ENDCG
        }

    }
}
