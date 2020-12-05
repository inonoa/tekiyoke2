Shader "Unlit/LogoDissolve"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DissolveTex ("Dissolve Texture", 2D) = "white" {}
        _DissolveThreshold0 ("Dissolve Threshold 0", Range(-1,2)) = 0
        _DissolveThreshold1 ("Dissolve Threshold 1", Range(-1,2)) = 1
        _GradationThreshold0("Gradation Threshold 0", Range(-1,2)) = 0
        _GradationThreshold1("Gradation Threshold 1", Range(-1,2)) = 1
        _EdgeColor ("Edge Color", Color) = (0,1,1,1)
        _Black2SpriteCol ("Black to Sprite Color", Range(0,1)) = 0
        _Black ("Black", Color) = (0,0,0,1)
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
            sampler2D _DissolveTex;
            float _DissolveThreshold0;
            float _DissolveThreshold1;
            float _GradationThreshold0;
            float _GradationThreshold1;
            fixed4 _EdgeColor;
            float _Black2SpriteCol;
            fixed4 _Black;

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

            float hashi(float v){
                return v > 0.5 ? (2 - 2 * v) * (2 - 2 * v) : 4 * v * v;
            }

            fixed4 frag (VertToFrag input) : SV_Target
            {
                fixed4 mainCol = tex2D(_MainTex, input.uv);
                mainCol.rgb = _Black2SpriteCol * mainCol.rgb + (1 - _Black2SpriteCol) * _Black.rgb;
                fixed4 col = mainCol;

                fixed4 dissolveCol = tex2D(_DissolveTex, input.uv);
                float dissolveAlpha = saturate((dissolveCol.r - _DissolveThreshold0) / (_DissolveThreshold1 - _DissolveThreshold0));
                col.a *= dissolveAlpha;

                fixed4 gradAlpha = saturate((input.uv.x - _GradationThreshold0) / (_GradationThreshold1 - _GradationThreshold0));
                col.a *= gradAlpha;

                col += _EdgeColor * hashi(dissolveAlpha * gradAlpha) * mainCol.a;

                return col * input.color;
            }
            ENDCG
        }
    }
}
