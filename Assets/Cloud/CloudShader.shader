Shader "Unlit/CloudShader"
{
    Properties
    {
        [PreRenderData] _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1, 1, 1, 1)
        _PerlinTexture ("Perlin Noise Texture", 2D) = "white" {}
        _VoronoiTexture ("Voronoi Noise Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
            Stencil
            {
                Ref 5
                Comp NotEqual
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 color: COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color: COLOR;
            };

            sampler2D _MainTex;
            sampler2D _PerlinTexture;
            sampler2D _VoronoiTexture;
            float4 _MainTex_ST;
            float4 _PerlinTexture_ST;
            float4 _VoronoiTexture_ST;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
                return o;
            }

            float circle (float radius, float2 p)
            {
                float value = distance(p, float2(0.5, 0.5));
                value = smoothstep(0.0, radius, value);
                return smoothstep(0.0, 1.0, 1.0 - value);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float cir = circle(0.5, i.uv);
                float2 static_uv = i.uv;
                i.uv += float2(_Time.y / 25.0, 0.0);
                fixed4 worley = tex2D(_VoronoiTexture, static_uv);
                worley.rgb = smoothstep(0.0, 1.0, 1.0 - worley.rgb);
                fixed4 perlin = tex2D(_PerlinTexture, i.uv * 4);
                fixed4 main_tex = tex2D(_MainTex, i.uv);
                fixed4 color = clamp(0, 1, worley * perlin + 0.5);
                color.a = (color.r + 0.4) * cir * i.color.a;
                color.rgb = main_tex.rgb;
                return color;
            }
            ENDCG
        }
    }
}
