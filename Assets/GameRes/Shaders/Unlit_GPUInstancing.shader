Shader "CustonRP/Unlit_GPUInstancing"
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
            #pragma multi_compile_instancing
            #pragma vertex UnlitVertexShader
            #pragma fragment UnlitFragShader
            #include "UnlitPass_GPUInstancing.hlsl"

            ENDHLSL
        }
    }
}
