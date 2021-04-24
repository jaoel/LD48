Shader "Unlit/Ground"
{
    Properties
    {
        _StartColor("Start Color", Color) = (1, 1, 1, 1)
        _EndColor("End Color", Color) = (1, 1, 1, 1)
    }
        SubShader
        {
            Tags {
                "RenderType" = "Opaque"
            }
            LOD 100

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
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                float4 _StartColor;
                float4 _EndColor;
                float4 _Color;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = lerp(_StartColor, _EndColor, 1 - i.uv.y) * i.uv.x;
                    return col;
                }

                ENDCG
            }
        }
}
