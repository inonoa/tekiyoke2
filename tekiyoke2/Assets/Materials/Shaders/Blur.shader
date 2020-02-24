Shader "Unlit/Blur"
{
  Properties
  {
    _MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
    _Distance ("Distance", Float) = 0.015
    _Resolution ("Resolution", Int) = 2
  }

  SubShader
  {
    LOD 100

    Tags
    {
      "Queue" = "Transparent"
      "IgnoreProjector" = "True"
      "RenderType" = "Transparent"
    }

    Cull Off
    Lighting Off
    ZWrite Off
    Fog { Mode Off }
    Offset -1, -1
    Blend SrcAlpha OneMinusSrcAlpha

    Pass
    {
      CGPROGRAM
      #pragma vertex vertexProgram
      #pragma fragment fragmentProgram

      #include "UnityCG.cginc"

      struct appdata_t
      {
        float4 vertex : POSITION;
        float2 textureCoordinate : TEXCOORD0;
        fixed4 color : COLOR;
      };

      struct vertexToFragment
      {
        float4 vertex : SV_POSITION;
        half2 textureCoordinate : TEXCOORD0;
        fixed4 color : COLOR;
      };

      sampler2D _MainTex;
      float4 _MainTex_ST;
      float _Distance;
      int _Resolution;

      vertexToFragment vertexProgram (appdata_t vertexData)
      {
        vertexToFragment output;
        output.vertex = UnityObjectToClipPos(vertexData.vertex);
        output.textureCoordinate = TRANSFORM_TEX(vertexData.textureCoordinate, _MainTex);
        output.color = vertexData.color;
        return output;
      }

      fixed4 fragmentProgram (vertexToFragment input) : COLOR
      {
        float distance = _Distance / _Resolution;
        int resolution = _Resolution;
        fixed4 computedColor = tex2D(_MainTex, input.textureCoordinate) * input.color;
        //なんかこう……高速にならないですかね……
        for(int i=-resolution; i<resolution+1; i++){
            for(int j=-resolution; j<resolution+1; j++){
                computedColor += tex2D(_MainTex, half2(
                    input.textureCoordinate.x + distance * i ,
                    input.textureCoordinate.y + distance * j
                    )) * input.color;
            }
        }
        computedColor = computedColor / (2*resolution + 1) / (2*resolution + 1);

        return computedColor;
      }
      ENDCG
    }
  }
}