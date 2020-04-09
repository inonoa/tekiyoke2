Shader "Unlit/EdgeLight"
{
    Properties
    {
        _MainTex ("Base", 2D) = "white" {}
        _LightColor ("Light Color", Color) = (0.2,0,1, 1)
        _Volume ("Volume", float) = 1
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
                fixed4 baseClr = tex2D(_MainTex, output.uv);

                float alphaSum = 0;

                fixed4 unitDist = 0.01 * _Volume;
                for(int i=-2;i<3;i++){
                    for(int j=-2;j<3;j++){
                        alphaSum += tex2D(_MainTex, output.uv + float2(i, j) * unitDist).a;
                    }
                }

                float lightVol = (alphaSum <= 10) ? alphaSum/10 : (15-alphaSum)/5;
                lightVol = _Volume==0 ? 0 : saturate(lightVol);

                fixed4 col = baseClr + lightVol * _LightColor;

                return col;
            }
            ENDCG
        }

    }
}
