Shader "Hidden/DraftWind"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma instancing_options procedural:setup

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            struct Piece
            {
                float3 pos;
                float3 vel;
            };

            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
            StructuredBuffer<Piece> _Pieces;
            #endif

            float random (fixed2 p)
            {
                return frac(sin(dot(p, fixed2(12.9898,78.233))) * 43758.5453);
            }

            void setup()
            {
                
            }

            v2f vert (appdata v, uint instanceID : SV_InstanceID)
            {
                v2f o;

                #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
                o.vertex = UnityObjectToClipPos(_Pieces[instanceID].pos + v.vertex);
                #else
                o.vertex = UnityObjectToClipPos(v.vertex);
                #endif

                o.uv     = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
