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

        Cull Off
        ZWrite Off
        Blend SrcAlpha One

        Pass
        {
            CGPROGRAM
            #pragma target 5.0
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            #pragma multi_compile_instancing
            #pragma instancing_options procedural:setup

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2g
            {
                float4 vertex : POSITION1;

                float4 pos  : SV_POSITION;
                float uv_x : TEXCOORD0;

                float2 lastPos  : POSITION2;
                float lastUv_x : TEXCOORD1;
            };

            struct g2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4    _MainTex_ST;
            float4    _Color;
            int       _NumNodesPerWind;
            float     _NodeLife;
            float     _Time_;
            float     _Width;

            struct Wind
            {
                int currentIndex;
                float angle;
            };

            struct Node
            {
                float  time;
                float2 pos;
            };

            StructuredBuffer<Wind> _Winds;
            StructuredBuffer<Node> _Nodes;

            float random (fixed2 p)
            {
                return frac(sin(dot(p, fixed2(12.9898,78.233))) * 43758.5453);
            }

            void setup()
            {
                
            }

            float4 ToClipPos(float2 pos, float4 vertex)
            {
                return UnityObjectToClipPos(float4(pos, 0, 0) + vertex);
            }

            bool invalidNode(uint idx)
            {
                return _Nodes[idx].time == -1;
            }

            v2g vert (appdata v, uint nodeId : SV_VertexID, uint instanceID : SV_InstanceID)
            {
                v2g o;
                o.vertex = v.vertex;

                uint nodeId_    =  nodeId                                            + instanceID * _NumNodesPerWind;
                uint lastNodeId = (nodeId - 1 + _NumNodesPerWind) % _NumNodesPerWind + instanceID * _NumNodesPerWind;

                o.pos     = float4(_Nodes[nodeId_].pos, 0, 0);
                o.lastPos = float4(invalidNode(lastNodeId) ? o.pos : _Nodes[lastNodeId].pos, 0, 0);

                o.uv_x     = invalidNode(nodeId_)    ? -1 : ((_Time_ - _Nodes[nodeId_].time)    / _NodeLife);
                o.lastUv_x = invalidNode(lastNodeId) ? -1 : ((_Time_ - _Nodes[lastNodeId].time) / _NodeLife);

                return o;
            }

            [maxvertexcount(4)]
            void geom(point v2g input[1], inout TriangleStream<g2f> output)
            {
                v2g ip = input[0];

                if((ip.uv_x < 0) || (ip.lastUv_x < 0)) return;

                float2 dir2last = normalize(ip.lastPos.xy - ip.pos.xy); //posとlastPos同じだとやばそう
                float2 dirCrossing = float2(dir2last.y, -dir2last.x);
                float2 widthOffset = dirCrossing * _Width * 0.5;

                g2f vert0;
                vert0.uv  = float2(ip.uv_x, 0);
                vert0.pos = ToClipPos(ip.pos.xy + widthOffset, ip.vertex);

                g2f vert1;
                vert1.uv  = float2(ip.uv_x, 1);
                vert1.pos = ToClipPos(ip.pos.xy - widthOffset, ip.vertex);

                g2f vertLast0;
                vertLast0.uv  = float2(ip.lastUv_x, 0);
                vertLast0.pos = ToClipPos(ip.lastPos.xy + widthOffset, ip.vertex);

                g2f vertLast1;
                vertLast1.uv  = float2(ip.lastUv_x, 1);
                vertLast1.pos = ToClipPos(ip.lastPos.xy - widthOffset, ip.vertex);

                bool gyaku = ip.pos.y < ip.lastPos.y;
            
                output.Append(vertLast0);
                output.Append(vertLast1);
                output.Append(vert0);
                output.Append(vert1);
                output.RestartStrip();
            }


            fixed4 frag (g2f i) : COLOR
            {
                fixed4 col = tex2D(_MainTex, i.uv) * _Color;
                return col;
            }
            ENDCG
        }
    }
}
