using System;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRenderer
{
    private const string BufferName = "RenderCamera";
    private static ShaderTagId s_unlitShaderTag = new ShaderTagId("SRPDefaultUnlit");

    private CommandBuffer m_buffer = new CommandBuffer() { name = BufferName};
    private ScriptableRenderContext m_context;
    // 存储剔除后的结果
    private CullingResults m_cullingResults;
    private Camera m_camera = null;

    public void Render(ScriptableRenderContext context, Camera camera)
    {
        m_context = context;
        m_camera = camera;

#if UNITY_EDITOR
        PrepareBuffer();
        PrepareForSceneWindow();
#endif

        if (!Cull())
            return;

        SetUp();
        
        DrawVisibleGeometry();
#if UNITY_EDITOR
        DrawUnsupportedShaders();
        DrawGizmos();
#endif
        Submit();
    }

    private bool Cull()
    {
        if (m_camera.TryGetCullingParameters(out var cullingParams))
        {
            m_cullingResults = m_context.Cull(ref cullingParams);
            return true;
        }
        return false;
    }

    private void SetUp()
    {
        // 更新相机矩阵
        m_context.SetupCameraProperties(m_camera);
        // 依据相机的clearFlags来清理渲染缓冲区
        CameraClearFlags flag = m_camera.clearFlags;
        bool clearDepth = flag <= CameraClearFlags.Depth;
        // SkyBox就不用清理了，最终会渲一遍天空盒，也就自动清理了
        bool clearColor = flag == CameraClearFlags.Color;
        Color color = clearColor ? m_camera.backgroundColor.linear : Color.clear;
        m_buffer.ClearRenderTarget(clearDepth, clearColor, color);
        m_buffer.BeginSample(BufferName);
        ExecuteBuffer();
    }

    private void DrawVisibleGeometry()
    {
        // 设置渲染排序规则为不透明对象排序，开始渲染不透明物体。
        SortingSettings sortingSettings = new SortingSettings()
        { 
            criteria = SortingCriteria.CommonOpaque,
        };
        // 设置渲染的shader pass和渲染排序规则
        DrawingSettings drawingSettings = new DrawingSettings(s_unlitShaderTag, sortingSettings);
        // 渲染对象的过滤设置
        FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
        // 渲染不透明物体
        m_context.DrawRenderers(m_cullingResults, ref drawingSettings, ref filteringSettings);
        // 渲染天空盒
        m_context.DrawSkybox(m_camera);
        // 设置渲染排序规则为不透明对象排序，开始渲染不透明物体。
        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;
        // 渲染透明物体
        m_context.DrawRenderers(m_cullingResults, ref drawingSettings, ref filteringSettings);
    }

    private void Submit()
    {
        m_buffer.EndSample(BufferName);
        ExecuteBuffer();
        m_context.Submit();
    }

    private void ExecuteBuffer()
    { 
        m_context.ExecuteCommandBuffer(m_buffer);
        m_buffer.Clear();
    }

}
