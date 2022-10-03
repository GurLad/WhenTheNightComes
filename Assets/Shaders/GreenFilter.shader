Shader "Custom/GreenFilter"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BrigthnessMod("Brightness mod", Range(0.0, 1.0)) = 0.2
        _GreenMulti("Green multi", Range(0.0, 1.0)) = 0.2
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _BrigthnessMod;
            float _GreenMulti;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // Apperantly, colors can exceed 1 for no apperant reason                
                if (col.r >= 1)
                {
                    col.r = 1;
                }
                if (col.g >= 1)
                {
                    col.g = 1;
                }
                if (col.b >= 1)
                {
                    col.b = 1;
                }
                float brightness = col.r * 0.3 + col.g * 0.59 + col.b * 0.11;
                col.g += brightness * _BrigthnessMod;
                col.g *= (1 + _GreenMulti);
                if (col.g >= 1)
                {
                    col.g = 1;
                }
                return col;
            }
            ENDCG
        }
    }
}
