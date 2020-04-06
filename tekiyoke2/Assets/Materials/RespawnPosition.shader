Shader "Unlit/RespawnPosition"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Volume ("Volume", float) = 0.2
        _Density("Density", float) = 0.025
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 100

        GrabPass { }

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
            float _Volume;
            float _Density;
            sampler2D _GrabTexture;

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

            float odoriba(float x){
                return (x <= 1 - _Density) ? (x < _Density) ? (x / _Density / 2) : 0.5 : ( (x-1) / _Density / 2 + 1);
            }

            float hashi(float v){
                return v > 0.5 ? 3 - 2 * v : 2 * v;
            }

            fixed4 frag (VertToFrag input) : SV_Target
            {
                float2 grabUv = (input.bgPos.xy / input.bgPos.w);
                float2 uvZure = float2(random(_SinTime.w), random(_SinTime.y));
                fixed4 bgCol = tex2D(_GrabTexture, grabUv);
                fixed4 bgZureCol = tex2D(_GrabTexture, grabUv + uvZure / 200);

                fixed4 col = tex2D(_MainTex, input.uv);
                float zureness = 1 - col.r;

                fixed4 light = fixed4(random(_SinTime.x) * 0.5, 0, random(_SinTime.z), 0) * hashi(1-col.r);

                return zureness * bgZureCol + (1 - zureness) * bgCol + light;
            }
            ENDCG
        }
    }
}
