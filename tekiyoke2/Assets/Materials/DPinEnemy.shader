Shader "Unlit/DPinEnemy"
{
    Properties
    {
        _MainTex ("Base", 2D) = "white" {}
        _LightTex ("Light Texture", 2D) = "white" {}
        _Volume ("Volume", Range(0,1)) = 0.1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
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
            sampler2D _LightTex;
            float _Volume;

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
                fixed4 mainCol = tex2D(_MainTex, output.uv);
                fixed4 lightCol = tex2D(_LightTex, output.uv);

                return (lightCol * _Volume + mainCol * (1 - _Volume)) * output.color;
            }
            ENDCG
        }

    }
}
