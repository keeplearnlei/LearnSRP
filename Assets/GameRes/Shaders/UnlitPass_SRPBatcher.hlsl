#ifndef CUSTON_UNLIT_PASS_SRP_BATCHER
#define CUSTON_UNLIT_PASS_SRP_BATCHER
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
#endif // CUSTON_UNLIT_PASS_SRP_BATCHER