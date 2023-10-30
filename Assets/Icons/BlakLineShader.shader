Shader "Custom/IntermittentLineShader"
{
    Properties
    {
        _Color ("Line Color", Color) = (1, 1, 1, 1)
        _LineWidth ("Line Width", Range(0.01, 0.5)) = 0.1
        _Spacing ("Spacing", Range(0.1, 2.0)) = 0.5
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
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

            fixed4 _Color;
            float _LineWidth;
            float _Spacing;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // Calculate the distance from the start of the line
                float distance = frac(i.uv.x / _Spacing);

                // Determine whether to make this segment opaque or transparent
                half4 color = (distance <= _LineWidth) ? _Color : half4(0, 0, 0, 0);

                return color;
            }
            ENDCG
        }
    }
}
