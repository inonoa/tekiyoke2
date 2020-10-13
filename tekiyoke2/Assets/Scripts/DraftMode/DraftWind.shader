Shader "Hidden/DraftWind"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue"      = "Transparent"
        }

        // Tags{ "RenderType" = "Opaque" }

        LOD 100

        ZWrite Off
        Blend SrcAlpha One

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
            float4 _Color;
            int _NumNodesPerWind;

            struct Wind
            {
                int currentIndex;
            };

            struct Node
            {
                float  time;
                float2 pos;
            };

            #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
            StructuredBuffer<Wind> _Winds;
            StructuredBuffer<Node> _Nodes;
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
                uint id = _Winds[instanceID].currentIndex + _NumNodesPerWind * instanceID;
                o.vertex = UnityObjectToClipPos(float3(_Nodes[id].pos, 0) + v.vertex);
                #else
                o.vertex = UnityObjectToClipPos(v.vertex);
                #endif

                o.uv     = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDCG
        }
    }
}
