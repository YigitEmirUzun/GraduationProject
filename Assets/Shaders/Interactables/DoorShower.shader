Shader "Custom/URP_ExitBeam"
{
    Properties
    {
        [HDR] _Color ("Glow Color", Color) = (0.3, 0.8, 1.0, 1)
        _Intensity ("Intensity", Range(0, 5)) = 1.5

        [Header(Vertical Fade)]
        _BottomY ("Bottom Y (object space)", Float) = -0.5
        _TopY ("Top Y (object space)", Float) = 0.5
        _FadePower ("Fade Power", Range(0.2, 5)) = 1.5

        [Header(Pulse)]
        _PulseSpeed ("Pulse Speed", Range(0, 6)) = 1.5
        _PulseAmount ("Pulse Amount", Range(0, 1)) = 0.25

        [Header(Edge Glow)]
        _EdgePower ("Edge Power", Range(0.2, 8)) = 2.5
        _EdgeAmount ("Edge Amount", Range(0, 2)) = 0.8
        _BaseFill ("Inner Fill", Range(0, 1)) = 0.25

        [Header(Rising Bands)]
        _BandAmount ("Rising Bands", Range(0, 1)) = 0.25
        _BandScale ("Band Scale", Range(1, 30)) = 8
        _BandSpeed ("Band Speed", Range(0, 3)) = 0.4
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            Name "ExitBeam"
            Tags { "LightMode"="UniversalForward" }

            Blend SrcAlpha One      // additive glow
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                half _Intensity;
                half _BottomY;
                half _TopY;
                half _FadePower;
                half _PulseSpeed;
                half _PulseAmount;
                half _EdgePower;
                half _EdgeAmount;
                half _BaseFill;
                half _BandAmount;
                half _BandScale;
                half _BandSpeed;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS   : TEXCOORD0;
                float3 viewDirWS  : TEXCOORD1;
                float  heightOS   : TEXCOORD2;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs pos = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionCS = pos.positionCS;
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.viewDirWS = GetWorldSpaceViewDir(pos.positionWS);
                OUT.heightOS = IN.positionOS.y;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                // ---- Yukarý doðru saydamlaþma ----
                half t = saturate((IN.heightOS - _BottomY) / max(_TopY - _BottomY, 1e-4));
                half fade = pow(saturate(1.0h - t), _FadePower);

                // ---- Hafif, düzensiz nefes alan glow ----
                half p1 = sin(_Time.y * _PulseSpeed);
                half p2 = sin(_Time.y * _PulseSpeed * 0.37h + 1.3h);
                half pulse = 1.0h + (p1 * 0.6h + p2 * 0.4h) * _PulseAmount;

                // ---- Kenar parlamasý (fresnel) ----
                half3 N = normalize(IN.normalWS);
                half3 V = normalize(IN.viewDirWS);
                half fres = pow(1.0h - saturate(abs(dot(N, V))), _EdgePower);
                half body = _BaseFill + fres * _EdgeAmount;

                // ---- Yukarý süzülen hafif bantlar ----
                half band = sin((IN.heightOS * _BandScale) - _Time.y * _BandSpeed * 6.2832h) * 0.5h + 0.5h;
                half bands = lerp(1.0h, 0.6h + band * 0.8h, _BandAmount);

                half alpha = saturate(fade * body * bands * pulse);
                half3 col = _Color.rgb * _Intensity * pulse;

                return half4(col, alpha);
            }
            ENDHLSL
        }
    }
}