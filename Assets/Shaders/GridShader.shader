Shader "Custom/GridShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GridColor ("Grid Color", Color) = (1, 1, 1, 1)
        _GridSpacing ("Grid Spacing", Range(0.1, 10)) = 1.0
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
            #include "UnityCG.cginc"
 
            struct appdata_t
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
            float4 _GridColor;
            float _GridSpacing;
 
            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
 
            half4 frag (v2f i) : SV_Target
            {
                float2 gridUV = i.uv * _GridSpacing;
                float2 grid = fmod(gridUV, 1.0);
                half4 gridColor = lerp(_GridColor, half4(1, 1, 1, 1), step(0.02, min(grid.x, grid.y)));
                half4 texColor = tex2D(_MainTex, i.uv);
                return texColor * gridColor;
            }
            ENDCG
        }
    }
}
