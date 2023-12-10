#ifndef CUSTON_UNLIT_PASS_GPU_INSTANCING
#define CUSTON_UNLIT_PASS_GPU_INSTANCING
#include "./ShaderLibrarys/Common.hlsl"

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
UNITY_DEFINE_INSTANCED_PROP(float4, _BaseColor)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

struct Attributes{
    float3 positionOS:POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings{
    float4 positionCS:SV_POSITION;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

Varyings UnlitVertexShader(Attributes input)
{
    Varyings output;
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);
    float3 positionWS = TransformObjectToWorld(input.positionOS);
    output.positionCS = TransformWorldToHClip(positionWS);
    return output;
}

float4 UnlitFragShader(Varyings input):SV_TARGET0
{
    UNITY_SETUP_INSTANCE_ID(input);

    return UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseColor);
}

#endif // CUSTON_UNLIT_PASS_GPU_INSTANCING