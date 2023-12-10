using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
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

    public void PrepareBuffer()
    {
        Profiler.BeginSample("EditorOnly");
        m_buffer.name = m_camera.name;
        Profiler.EndSample();
    }

    // 解决UI无法在Scene视图绘制的问题
    private void PrepareForSceneWindow()
    {
        if (m_camera.cameraType == CameraType.SceneView)
        {
            ScriptableRenderContext.EmitWorldGeometryForSceneView(m_camera);
        }
    }

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

    private void DrawGizmos()
    {
        if (Handles.ShouldRenderGizmos())
        {
            m_context.DrawGizmos(m_camera, GizmoSubset.PreImageEffects);
            m_context.DrawGizmos(m_camera, GizmoSubset.PostImageEffects);
        }
    }
}
#endif