Shader "Custom/EdgeLines"
{
    Properties
    {
        _LineColor ("Line Color", Color) = (0.05,0.05,0.08,1)
        _Thickness ("Thickness (px, tam sayý)", Range(1,4)) = 1
        _DepthThreshold ("Depth Threshold (metre)", Range(0.01,2)) = 0.15
        _NormalThreshold ("Normal Threshold", Range(0.05,2)) = 0.6
    }
    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" }
        Pass
        {
            ZTest Always ZWrite Off Cull Off

            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"

            float4 _LineColor;
            float _Thickness, _DepthThreshold, _NormalThreshold;

            // Ortografik kamerada derinlik lineer, perspektifte deðil
            float EyeDepth(float2 uv)
            {
                float raw = SampleSceneDepth(uv);
                if (unity_OrthoParams.w > 0.5)
                {
                    #if UNITY_REVERSED_Z
                        raw = 1.0 - raw;
                    #endif
                    return lerp(_ProjectionParams.y, _ProjectionParams.z, raw);
                }
                return LinearEyeDepth(raw, _ZBufferParams);
            }

            bool IsSky(float2 uv)
            {
                float raw = SampleSceneDepth(uv);
                #if UNITY_REVERSED_Z
                    return raw < 0.00001;
                #else
                    return raw > 0.99999;
                #endif
            }

            half4 frag(Varyings i) : SV_Target
            {
                float2 uv = i.texcoord;
                half4 col = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv);

                // Gökyüzü/boþlukta çizgi çizme
                if (IsSky(uv)) return col;

                float2 o = max(1.0, round(_Thickness)) / _ScreenParams.xy;
                float2 uvL = uv + float2(-o.x, 0);
                float2 uvR = uv + float2( o.x, 0);
                float2 uvU = uv + float2(0,  o.y);
                float2 uvD = uv + float2(0, -o.y);

                float dC = EyeDepth(uv);
                float dL = EyeDepth(uvL), dR = EyeDepth(uvR);
                float dU = EyeDepth(uvU), dD = EyeDepth(uvD);

                // Ýþaretli ikinci türev: sadece kenarýn ARKADAKÝ tarafý pozitif çýkar (1 piksel çizgi)
                float metric;
                if (unity_OrthoParams.w > 0.5)
                {
                    float lapX = dL + dR - 2.0 * dC;
                    float lapY = dU + dD - 2.0 * dC;
                    metric = max(max(-lapX, -lapY), 0.0);             // metre cinsinden
                }
                else
                {
                    float iC = 1.0 / dC, iL = 1.0 / dL, iR = 1.0 / dR, iU = 1.0 / dU, iD = 1.0 / dD;
                    float lapX = iL + iR - 2.0 * iC;
                    float lapY = iU + iD - 2.0 * iC;
                    float iMax = max(iC, max(max(iL, iR), max(iU, iD)));
                    metric = max(max(lapX, lapY), 0.0) / iMax * 20.0;
                }

                float depthEdge = smoothstep(_DepthThreshold * 0.6, _DepthThreshold, metric);

                // Gökyüzüne komþu obje pikseli: dýþ hat çizgisi
                if (IsSky(uvL) || IsSky(uvR) || IsSky(uvU) || IsSky(uvD)) depthEdge = 1.0;

                // Normal farký: merkez ile sað/üst komþu (tek taraflý, 1 piksel)
                float3 nC = SampleSceneNormals(uv);
                float3 nR = SampleSceneNormals(uvR);
                float3 nU = SampleSceneNormals(uvU);
                float nDiff = length(nR - nC) + length(nU - nC);
                float normalEdge = smoothstep(_NormalThreshold * 0.6, _NormalThreshold, nDiff);

                float edge = max(depthEdge, normalEdge);
                return lerp(col, _LineColor, edge * _LineColor.a);
            }
            ENDHLSL
        }
    }
}