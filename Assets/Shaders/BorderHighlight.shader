Shader "Custom/BorderHighlight" {
    Properties {
        _MainTex ("Albedo (RGB)", 2D) = "green" {}
        _BorderColor ("Border Color", Color) = (0,0,0,1)
        _BorderThickness ("Border Thickness", Range(0,0.5)) = 0.05
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _BorderColor;
            float _BorderThickness;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target {
                half2 uv = i.uv;
                half valX = smoothstep(_BorderThickness, 0, uv.x) - smoothstep(1, 1 - _BorderThickness, uv.x);
                half valY = smoothstep(_BorderThickness, 0, uv.y) - smoothstep(1, 1 - _BorderThickness, uv.y);
                half isBorder = min(valX, valY);
                half4 texColor = tex2D(_MainTex, uv);
                return lerp(texColor, _BorderColor, isBorder);
            }
            ENDCG
        }
    }
}
