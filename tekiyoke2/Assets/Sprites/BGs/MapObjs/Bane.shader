Shader "Unlit/Bane"
{
    Properties
    {
        _MainTex ("Base", 2D) = "white" {}
        _ColorOff ("Color (Hero Not On)", Color) = (0,1,1, 0.3)
        _ColorOn ("Color (Hero On)", Color) = (0,1,1, 0.3)
        [MaterialToggle] _HeroOn ("Hero On", int) = 0
        [MaterialToggle] _Flash ("Flash", int) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
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
            fixed4 _ColorOff;
            fixed4 _ColorOn;
            int _HeroOn;
            int _Flash;

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
                bool heroOn = (_HeroOn == 1);

                fixed4 col = fixed4(0,0,0,0);

                float wv = saturate(_Flash ? 1 : 0.7 * sin(output.uv.y * 20 - _Time.w) + 0.3);

                col += (heroOn ? _ColorOn : _ColorOff) * wv * (heroOn ? _ColorOn : _ColorOff).a;
                col.r = saturate(col.r);
                col.g = saturate(col.g);
                col.b = saturate(col.b);
                col.a = saturate(col.a);
                col.a *= saturate((1 - output.uv.y) * 1.25);
                col.a *= saturate(10 - 20 * abs(0.5 - output.uv.x));

                return col;
            }
            ENDCG
        }

    }
}
