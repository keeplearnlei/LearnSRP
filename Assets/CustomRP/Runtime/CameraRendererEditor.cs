using System;
using UnityEngine;
using UnityEngine.Rendering;

#if UNITY_EDITOR
public partial class CameraRenderer
{
    private static ShaderTagId[] s_unsupportedShaderTags = new ShaderTagId[] { 
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vectex"),
        new ShaderTagId("VectexLMRGBM"),
        new ShaderTagId("VectexLM"),
    };

    private Material m_errorMaterial = null;

    private void DrawUnsupportedShaders()
    {
        if (m_errorMaterial == null)
        {
            m_errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
        }
        DrawingSettings drawingSettings = new DrawingSettings(s_unsupportedShaderTags[0], new SortingSettings(m_camera));
        drawingSettings.overrideMaterial = m_errorMaterial;
        for (int i = 1; i < s_unsupportedShaderTags.Length; i++)
        { 
            drawingSettings.SetShaderPassName(i, s_unsupportedShaderTags[i]);
        }
        FilteringSettings filteringSettings = FilteringSettings.defaultValue;
        // 渲染不支持的Geometry
        m_context.DrawRenderers(m_cullingResults, ref drawingSettings, ref filteringSettings);
    }
}
#endif