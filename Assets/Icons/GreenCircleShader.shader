Shader "Custom/GreenCircle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CircleColor ("Circle Color", Color) = (0, 1, 0, 1)
        _Radius ("Circle Radius", Range(0, 0.5)) = 0.25
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
            float4 _CircleColor;
            float _Radius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            half4 frag (v2f i) : SV_Target
            {
                half2 center = half2(0.5, 0.5);
                half dist = length(i.uv - center);
                half circle = step(dist, _Radius);
                
                half4 col = _CircleColor * circle + (1 - circle) * tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
