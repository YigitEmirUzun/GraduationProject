Shader "Custom/URP_SpriteRimLight"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        [Header(Hit Flash)]
        _HitFlashAmount ("Hit Flash Amount", Range(0, 1)) = 0
        _HitFlashColor ("Hit Flash Color", Color) = (1,1,1,1)
        _GlowIntensity ("Glow Intensity", Float) = 2

        [Header(Scene Lighting)]
        _LightInfluence ("Light Influence", Range(0, 1)) = 1
        _ShadowColor ("Darkest Color (no light)", Color) = (0.35, 0.3, 0.5, 1)
        _AmbientAmount ("Ambient Amount", Range(0, 2)) = 1
        _MainLightAmount ("Main Light Amount", Range(0, 2)) = 1
        _PointLightAmount ("Point Light Amount", Range(0, 3)) = 1

        [Header(Rim Light)]
        _RimWidth ("Rim Width (UV)", Range(0.001, 0.15)) = 0.03
        _RimIntensity ("Rim Intensity", Range(0, 5)) = 1.5
        _RimSoftness ("Rim Softness", Range(0.1, 3)) = 1
        _RimColorTint ("Rim Color Tint", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            Name "SpriteRim"
            Tags { "LightMode"="UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _FORWARD_PLUS
            #pragma multi_compile _ _CLUSTER_LIGHT_LOOP

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                half4 _Color;
                half _HitFlashAmount;
                half4 _HitFlashColor;
                half _GlowIntensity;
                half _LightInfluence;
                half4 _ShadowColor;
                half _AmbientAmount;
                half _MainLightAmount;
                half _PointLightAmount;
                half _RimWidth;
                half _RimIntensity;
                half _RimSoftness;
                half4 _RimColorTint;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
                half4 color       : COLOR;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                half4 color       : COLOR;
            };

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                VertexPositionInputs pos = GetVertexPositionInputs(IN.positionOS.xyz);
                OUT.positionCS = pos.positionCS;
                OUT.positionWS = pos.positionWS;
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                OUT.color = IN.color;
                return OUT;
            }

            // Tek bir ek ýþýðý iþler: aydýnlatma + rim
            void ProcessLight(Light l, float3 viewDir, float2 uv, half texAlpha,
                              float3 dp1, float3 dp2, float2 du1, float2 du2, float det,
                              float d11, float d12, float d22,
                              inout half3 lighting, inout half3 rimTotal)
            {
                half atten = l.distanceAttenuation;

                // Iþýk karakterin önündeyse ön yüzü aydýnlatýr, arkadaysa az
                half facing = saturate(dot(l.direction, viewDir) * 0.5h + 0.5h);
                lighting += l.color * atten * facing * _PointLightAmount;

                // Rim: ýþýk yönünü UV yönüne çevir
                float2 rhs = float2(dot(l.direction, dp1), dot(l.direction, dp2));
                float a = (rhs.x * d22 - rhs.y * d12) / det;
                float b = (rhs.y * d11 - rhs.x * d12) / det;
                float2 dirUV = a * du1 + b * du2;
                float len = length(dirUV);
                if (len < 1e-6) return;
                dirUV /= len;

                half aMid = SAMPLE_TEXTURE2D_LOD(_MainTex, sampler_MainTex,
                                uv + dirUV * _RimWidth * 0.5, 0).a;
                half aFar = SAMPLE_TEXTURE2D_LOD(_MainTex, sampler_MainTex,
                                uv + dirUV * _RimWidth, 0).a;

                half edge = texAlpha * (1.0h - 0.5h * (aMid + aFar));
                edge = pow(saturate(edge), _RimSoftness);

                rimTotal += l.color * edge * atten;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                half4 baseCol = tex * _Color * IN.color;

                float3 P = IN.positionWS;
                float3 viewDir = normalize(_WorldSpaceCameraPos - P);

                // ---------- Sahne ýþýðý ----------
                half3 lighting = _ShadowColor.rgb;
                lighting += SampleSH(viewDir) * _AmbientAmount;

                Light mainLight = GetMainLight();
                half mainFacing = saturate(dot(mainLight.direction, viewDir) * 0.5h + 0.5h);
                lighting += mainLight.color * mainFacing * _MainLightAmount;

                half3 rimTotal = 0;

                // Ekran türevleri (döngünün dýþýnda hesaplanýr)
                float3 dp1 = ddx(P);
                float3 dp2 = ddy(P);
                float2 du1 = ddx(IN.uv);
                float2 du2 = ddy(IN.uv);
                float d11 = dot(dp1, dp1);
                float d12 = dot(dp1, dp2);
                float d22 = dot(dp2, dp2);
                float det = d11 * d22 - d12 * d12 + 1e-12;

                #if defined(_ADDITIONAL_LIGHTS) || defined(_FORWARD_PLUS) || defined(_CLUSTER_LIGHT_LOOP)

                    uint pixelLightCount = GetAdditionalLightsCount();

                    #if USE_FORWARD_PLUS || USE_CLUSTER_LIGHT_LOOP
                    InputData inputData = (InputData)0;
                    inputData.positionWS = P;
                    inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.positionCS);

                    // Forward+ modunda ek yönlü ýþýklar ayrý döngüyle iþlenir
                    for (uint dirIndex = 0u;
                         dirIndex < min(URP_FP_DIRECTIONAL_LIGHTS_COUNT, MAX_VISIBLE_LIGHTS);
                         dirIndex++)
                    {
                        Light dl = GetAdditionalLight(dirIndex, P);
                        ProcessLight(dl, viewDir, IN.uv, tex.a, dp1, dp2, du1, du2,
                                     det, d11, d12, d22, lighting, rimTotal);
                    }
                    #endif

                    // Point / Spot ýþýklar (Forward ve Forward+ için ortak makro)
                    LIGHT_LOOP_BEGIN(pixelLightCount)
                        Light l = GetAdditionalLight(lightIndex, P);
                        ProcessLight(l, viewDir, IN.uv, tex.a, dp1, dp2, du1, du2,
                                     det, d11, d12, d22, lighting, rimTotal);
                    LIGHT_LOOP_END

                #endif

                // 0 = düz sprite, 1 = tamamen sahne ýþýðýna baðlý
                baseCol.rgb *= lerp(half3(1,1,1), lighting, _LightInfluence);

                // Rim, ýþýklandýrmadan sonra eklenir
                baseCol.rgb += rimTotal * _RimIntensity * _RimColorTint.rgb;

                // ---------- Hit flash (eski Sprite_Lit graph'ý ile ayný) ----------
                // 1) Base color flash rengine doðru lerp
                baseCol.rgb = lerp(baseCol.rgb, _HitFlashColor.rgb, _HitFlashAmount);
                // 2) Emission: FlashColor * Amount * GlowIntensity (Bloom için parlama)
                baseCol.rgb += _HitFlashColor.rgb * (_HitFlashAmount * _GlowIntensity);

                return baseCol;
            }
            ENDHLSL
        }
    }
}