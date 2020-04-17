Shader "Unlit/Glitch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _TimeRange ("Time Range", float) = 0.3
        _XRange ("X Range", float) = 0.3
        _YRange ("Y Range", float) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
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
                float4 bgPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float _TimeRange;
            float _XRange;
            float _YRange;

            VertToFrag vert (VertInput vert)
            {
                VertToFrag output;

                output.vertex = UnityObjectToClipPos(vert.vertex);
                output.uv     = vert.uv;
                output.bgPos  = ComputeGrabScreenPos(output.vertex);
                output.color  = vert.color;

                return output;
            }

            float random (fixed2 p){
                return frac(sin(dot(p, fixed2(12.9898,78.233))) * 43758.5453);
            }

            fixed4 frag (VertToFrag input) : SV_Target
            {
                //_Time依存でずれの境目を決める
                float timeSteppedBase = floor((_Time.y % 10) / _TimeRange);
                float timeSteppedNoise = random(_SinTime.zw) > 0.9 ? 1 : 0;
                float timeStepped = timeSteppedBase + timeSteppedNoise;
                float2 zureGap = float2(
                                     random(fixed2(timeStepped * 2, timeStepped)),
                                     random(fixed2(timeStepped * 3, timeStepped / 2))
                                 );

                //_Time, uv.x, uv.y(, zureGap)依存でずれ具合を決める
                float xStepped = floor((input.uv.x + zureGap.x) / _XRange) / 3;
                float yStepped = floor((input.uv.y + zureGap.y) / _YRange) * 1.3;

                float zureSeed = xStepped + yStepped + timeStepped;
                float zureW = random(fixed2(zureSeed,zureSeed*1.1)) * 0.05 - 0.025;
                float zureH = random(fixed2(zureSeed*1.4,zureSeed*0.9)) * 0.05 - 0.025;

                return tex2D(_MainTex, input.uv + float2(zureW, zureH)) * input.color;
            }
            ENDCG
        }
    }
}
