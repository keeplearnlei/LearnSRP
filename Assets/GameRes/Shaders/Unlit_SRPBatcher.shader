Shader "CustonRP/Unlit_SRPBatcher"
{
    Properties
    {
        _BaseColor("MainColor", Color) = (1.0, 1.0, 1.0, 1.0)
    }

    SubShader
    {
        Pass
        {
            HLSLPROGRAM
            #pragma vertex UnlitVertexShader
            #pragma fragment UnlitFragShader
            #include "UnlitPass_SRPBatcher.hlsl"
            #include "./ShaderLibrarys/Common.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float4 _BaseColor;
            CBUFFER_END

            float4 UnlitVertexShader(float3 positionOS:POSITION):SV_POSITION
            {
                float3 positionWS = TransformObjectToWorld(positionOS);
                return TransformWorldToHClip(positionWS);
            }

            float4 UnlitFragShader():SV_TARGET0
            {
                return _BaseColor;
            }

            ENDHLSL
        }
    }
}
