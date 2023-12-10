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

            ENDHLSL
        }
    }
}
