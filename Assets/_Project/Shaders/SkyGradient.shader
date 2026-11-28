// Cielo de atardecer: degradado del horizonte al cénit, suelo rosado y resplandor del sol.
Shader "HillSlingers/SkyGradient"
{
    Properties
    {
        _Zenith ("Cénit", Color) = (0.25, 0.18, 0.55, 1)
        _Horizon ("Horizonte", Color) = (1.0, 0.55, 0.60, 1)
        _Ground ("Suelo lejano", Color) = (0.85, 0.45, 0.60, 1)
        _SunColor ("Color del sol", Color) = (1.0, 0.80, 0.55, 1)
        _SunDir ("Dirección del sol", Vector) = (0.5, 0.15, 0.85, 0)
    }
    SubShader
    {
        Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _Zenith;
            fixed4 _Horizon;
            fixed4 _Ground;
            fixed4 _SunColor;
            float4 _SunDir;

            struct appdata { float4 vertex : POSITION; };
            struct v2f { float4 pos : SV_POSITION; float3 dir : TEXCOORD0; };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.dir = v.vertex.xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 d = normalize(i.dir);
                float h = d.y;
                fixed3 sky = lerp(_Horizon.rgb, _Zenith.rgb, pow(saturate(h), 0.55));
                fixed3 below = lerp(_Horizon.rgb, _Ground.rgb, saturate(-h * 4.0));
                fixed3 col = h >= 0 ? sky : below;

                float s = saturate(dot(d, normalize(_SunDir.xyz)));
                col += _SunColor.rgb * (pow(s, 600.0) * 3.0 + pow(s, 24.0) * 0.55 + pow(s, 5.0) * 0.18);
                return fixed4(col, 1);
            }
            ENDCG
        }
    }
}
