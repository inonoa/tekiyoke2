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
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma target 5.0
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata
            {
                
            };

            struct v2g
            {
                float2 pos  : TEXCOORD0;
                float2 dir  : TEXCOORD1;
                float  uv_x : TEXCOORD2;
                float4 col  : COLOR;

                float2 lastPos  : TEXCOORD3;
                float2 lastDir  : TEXCOORD4;
                float  lastUv_x : TEXCOORD5;
                float4 lastCol  : COLOR1;
            };

            struct g2f
            {
                float4 pos : SV_POSITION;
                float2 uv  : TEXCOORD0;
                float4 col : COLOR;
            };

            sampler2D _MainTex;
            float4    _MainTex_ST;
            float4    _Color;

            int       _NumNodesPerWind;
            float     _NodeLife;
            float     _WindWidth;

            float     _Time_;
            float4    _CenterPos;

            struct Wind
            {
                int   currentIndex;
                float angle;
                float velocity;
                int   state;
                float timeOffset;
                float goStraightSec;
                float rotateSec;
            };

            struct Node
            {
                float  time;
                float2 pos;
                float4 color;
            };

            StructuredBuffer<Wind> _Winds;
            StructuredBuffer<Node> _Nodes;

            float random (fixed2 p)
            {
                return frac(sin(dot(p, fixed2(12.9898,78.233))) * 43758.5453);
            }

            float4 toClipPos(float2 pos)
            {
                return UnityObjectToClipPos(float4(pos, 0, 0) + _CenterPos);
            }

            bool invalidNode(uint idx)
            {
                return _Nodes[idx].time < 0;
            }

            int nodeID(uint nodeIdInWind, uint windID)
            {
                return nodeIdInWind + windID * _NumNodesPerWind;
            }

            v2g vert (appdata v, uint nodeIdInWind : SV_VertexID, uint windID : SV_InstanceID)
            {
                v2g o;

                uint nodeId           = nodeID( nodeIdInWind,                                            windID);
                uint lastNodeId       = nodeID((nodeIdInWind - 1 + _NumNodesPerWind) % _NumNodesPerWind, windID);
                uint secondLastNodeId = nodeID((nodeIdInWind - 2 + _NumNodesPerWind) % _NumNodesPerWind, windID);

                bool nodeValid           = _Nodes[nodeId].time           > 0;
                bool lastNodeValid       = _Nodes[lastNodeId].time       > 0;
                bool secondLastNodeValid = _Nodes[secondLastNodeId].time > 0;

                o.pos     =                 _Nodes[nodeId].pos;
                o.lastPos = lastNodeValid ? _Nodes[lastNodeId].pos : o.pos;

                o.dir     =                       normalize(o.lastPos - o.pos);
                o.lastDir = secondLastNodeValid ? normalize(_Nodes[secondLastNodeId].pos - o.lastPos) : float2(0, 0);

                o.uv_x     =                 (_Time_ - _Nodes[nodeId].time)     / _NodeLife;
                o.lastUv_x = lastNodeValid ? (_Time_ - _Nodes[lastNodeId].time) / _NodeLife : -1;

                o.col     =                 _Nodes[nodeId].color;
                o.lastCol = lastNodeValid ? _Nodes[lastNodeId].color : float4(-1, -1, -1, -1);

                return o;
            }

            [maxvertexcount(4)]
            void geom(point v2g input[1], inout TriangleStream<g2f> output)
            {
                v2g ip = input[0];

                if((ip.uv_x < 0) || (ip.lastUv_x <= 0) || (distance(ip.pos, ip.lastPos) > 200)) return;

                float2 dirCrossing = float2(ip.dir.y, -ip.dir.x);
                float2 widthOffset = dirCrossing * _WindWidth * 0.5;

                float2 dirCrossingLast = float2(ip.lastDir.y, -ip.lastDir.x);
                float2 widthOffsetLast = ((dirCrossingLast == float2(0, 0)) ? widthOffset : dirCrossingLast * _WindWidth * 0.5);

                g2f vert0;
                vert0.uv  = float2(ip.uv_x, 0);
                vert0.pos = toClipPos(ip.pos + widthOffset);
                vert0.col = ip.col;

                g2f vert1;
                vert1.uv  = float2(ip.uv_x, 1);
                vert1.pos = toClipPos(ip.pos - widthOffset);
                vert1.col = ip.col;

                g2f vertLast0;
                vertLast0.uv  = float2(ip.lastUv_x, 0);
                vertLast0.pos = toClipPos(ip.lastPos + widthOffsetLast);
                vertLast0.col = ip.lastCol;

                g2f vertLast1;
                vertLast1.uv  = float2(ip.lastUv_x, 1);
                vertLast1.pos = toClipPos(ip.lastPos - widthOffsetLast);
                vertLast1.col = ip.lastCol;
            
                output.Append(vertLast0);
                output.Append(vertLast1);
                output.Append(vert0);
                output.Append(vert1);
                output.RestartStrip();
            }


            fixed4 frag (g2f i) : COLOR
            {
                fixed4 col = tex2D(_MainTex, i.uv) * i.col * _Color;
                return col;
            }
            ENDCG
        }
    }
}
