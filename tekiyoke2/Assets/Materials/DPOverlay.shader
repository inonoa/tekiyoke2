Shader "Unlit/DPOverlay"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WidthNormalized ("Overlay Width (0 ~ 1)", float) = 0.7
        _OverlayAlpha ("Overlay Alpha", float) = 0.7
        _OverlayColorOffset ("Overlay Color Offset (0 ~ 1)", float) = 0
        _BGAlpha ("BG Alpha", float) = 0.4
        _MainAlpha ("Main Alpha", float) = 0.7
        _GradationWidth ("Gradation Width", float) = 0.05
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
        }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
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
            float _WidthNormalized;
            float _OverlayAlpha;
            float _OverlayColorOffset;
            float _BGAlpha;
            float _MainAlpha;
            float _GradationWidth;

            VertToFrag vert (VertInput vert)
            {
                VertToFrag output;

                output.vertex = UnityObjectToClipPos(vert.vertex);
                output.uv     = vert.uv;
                output.color  = vert.color;

                return output;
            }

            // float random (fixed2 p){
            //     return frac(sin(dot(p, fixed2(12.9898,78.233))) * 43758.5453);
            // }

            fixed4 phase2OverlayColor(float phase){
                float phase_6 = (phase * 6) % 1;

                if(phase < (1/6.0)) return fixed4(1,0.3,0.3,1) * (1 - phase_6) + fixed4(1,1,0.3,1) * phase_6;
                if(phase < (2/6.0)) return fixed4(1,1,0.3,1) * (1 - phase_6) + fixed4(0.3,1,0.3,1) * phase_6;
                if(phase < (3/6.0)) return fixed4(0.3,1,0.3,1) * (1 - phase_6) + fixed4(0.3,1,1,1) * phase_6;
                if(phase < (4/6.0)) return fixed4(0.3,1,1,1) * (1 - phase_6) + fixed4(0.3,0.3,1,1) * phase_6;
                if(phase < (5/6.0)) return fixed4(0.3,0.3,1,1) * (1 - phase_6) + fixed4(1,0.3,1,1) * phase_6;
                                    return fixed4(1,0.3,1,1) * (1 - phase_6) + fixed4(1,0.3,0.3,1) * phase_6;
            }

            fixed4 frag (VertToFrag input) : SV_Target
            {
                fixed4 baseColor = tex2D(_MainTex, input.uv);

                //たまった部分の色を決める
                float phase = input.uv.x - _OverlayColorOffset;
                float phase_0_1 = (phase < 0) ? phase+1 : phase;
                fixed4 overlayColor = phase2OverlayColor(phase_0_1);
                fixed4 laidcolor = baseColor + overlayColor;

                float a_x = saturate((_WidthNormalized * (1 + _GradationWidth) - input.uv.x) / _GradationWidth);

                fixed4 color = _OverlayAlpha * a_x * laidcolor + (1 - _OverlayAlpha * a_x) * baseColor;
                color.a = (a_x * _MainAlpha + (1 - a_x) * _BGAlpha) * baseColor.a;

                return color;
            }
            ENDCG
        }
    }
}
