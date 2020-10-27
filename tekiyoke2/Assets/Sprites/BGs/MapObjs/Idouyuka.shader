Shader "Unlit/Idouyuka"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _OmoteColor("Omote Color", Color) = (0.3,0,  1,  1)
        _UraColor("Ura Color", Color)     = (  0,1,0.7,0.7)
        _Speed ("Speed", float) = 1
        _Shuki ("Shuki", int) = 2
        _SinThreshold ("Sin Threshold", float) = 0

        _FuyoSpeed ("Fuyo Speed", float) = 3
        _FuyoMaxHeight ("Fuyo Max Height", float) = 0.4
        _FuyoMinHeight ("Fuyo Min Height", float) = 0.2
        _FuyoShuki ("Fuyo Shuki", int) = 3
    }
    SubShader
    {
        Tags {
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

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float _Speed;
            int _Shuki;
            float _FuyoSpeed;
            float _FuyoMaxHeight;
            float _FuyoMinHeight;
            float4 _OmoteColor;
            float4 _UraColor;
            int _FuyoShuki;
            float _SinThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            float sinhigh(float x){
                return saturate((sin(x) - _SinThreshold) / (1 - _SinThreshold));
            }

            fixed4 frag (v2f i) : SV_Target
            {

                float theta = asin(2*i.uv.x - 1);
                float uraTheta = -theta + UNITY_PI;

                float omote = sinhigh(_Shuki*theta    + _Time.z * _Speed);
                float ura   = sinhigh(_Shuki*uraTheta + _Time.z * _Speed);

                float thresholdOmote = (_FuyoMaxHeight - _FuyoMinHeight)/2 * sin(_FuyoShuki * theta    - _Time.w * _FuyoSpeed)
                                            + (_FuyoMaxHeight + _FuyoMinHeight)/2;

                float thresholdUra   = (_FuyoMaxHeight - _FuyoMinHeight)/2 * sin(_FuyoShuki * uraTheta - _Time.w * _FuyoSpeed)
                                            + (_FuyoMaxHeight + _FuyoMinHeight)/2;

                fixed4 omoteCol = _OmoteColor * omote;
                omoteCol.a *= saturate(1 - (1-i.uv.y) / thresholdOmote);

                fixed4 uraCol = _UraColor * ura;
                uraCol.a *= saturate(1 - (1-i.uv.y) / thresholdUra);

                fixed4 col = max(uraCol, omoteCol);

                return ((i.uv.y + abs(i.uv.x-0.5) > 1.45) ? fixed4(0,0,0,0) : col);
            
            }
            ENDCG
        }
    }
}
