Shader "UI/FlashDefault"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _FlashColor ("Flash Color", Color) = (1,1,1,1)
        _FlashAmount ("Flash Amount", Range(0,1)) = 0

        _GlowSize ("Glow Size (px)", Range(0,20)) = 8
        _GlowIntensity ("Glow Intensity", Range(0,4)) = 2

        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata { float4 vertex : POSITION; float4 color : COLOR; float2 uv : TEXCOORD0; };
            struct v2f { float4 vertex : SV_POSITION; fixed4 color : COLOR; float2 uv : TEXCOORD0; float4 worldPos : TEXCOORD1; };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            fixed4 _Color, _FlashColor;
            float _FlashAmount, _GlowSize, _GlowIntensity;
            float4 _ClipRect;

            v2f vert(appdata v)
            {
                v2f o;
                o.worldPos = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv);
                fixed4 col = tex * i.color;
                col.rgb = lerp(col.rgb, _FlashColor.rgb, _FlashAmount);

                // Glow: çevredeki piksellerin alpha'sýný örnekle
                float2 px = _MainTex_TexelSize.xy * _GlowSize;
                float g = 0;
                [unroll] for (int k = 0; k < 12; k++)
                {
                    float ang = k * 0.5236; // 30 derece
                    float2 dir = float2(cos(ang), sin(ang)) * px;
                    g += tex2D(_MainTex, i.uv + dir).a;
                    g += tex2D(_MainTex, i.uv + dir * 0.5).a;
                }
                g = saturate(g / 12.0) * _GlowIntensity * _FlashAmount;
                float glowA = saturate(g) * i.color.a;

                // Sprite glow'un önünde
                float outA = col.a + glowA * (1 - col.a);
                float3 outRGB = (col.rgb * col.a + _FlashColor.rgb * glowA * (1 - col.a)) / max(outA, 0.0001);
                fixed4 result = fixed4(outRGB, outA);

                result.a *= UnityGet2DClipping(i.worldPos.xy, _ClipRect);
                return result;
            }
            ENDCG
        }
    }
}