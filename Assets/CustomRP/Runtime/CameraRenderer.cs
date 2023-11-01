using System;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRenderer
{
    private const string BufferName = "RenderCamera";
    private static ShaderTagId s_unlitShaderTag = new ShaderTagId("SRPDefaultUnlit");

    private CommandBuffer m_buffer = new CommandBuffer() { name = BufferName};
    private ScriptableRenderContext m_context;
    // �洢�޳���Ľ��
    private CullingResults m_cullingResults;
    private Camera m_camera = null;

    public void Render(ScriptableRenderContext context, Camera camera)
    {
        m_context = context;
        m_camera = camera;

        if (!Cull())
            return;

        SetUp();
        
        DrawVisibleGeometry();
#if UNITY_EDITOR
        DrawUnsupportedShaders();
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
        m_context.SetupCameraProperties(m_camera);
        m_buffer.ClearRenderTarget(true, true, Color.clear);
        m_buffer.BeginSample(BufferName);
        ExecuteBuffer();
    }

    private void DrawVisibleGeometry()
    {
        // ������Ⱦ�������Ϊ��͸���������򣬿�ʼ��Ⱦ��͸�����塣
        SortingSettings sortingSettings = new SortingSettings()
        { 
            criteria = SortingCriteria.CommonOpaque,
        };
        // ������Ⱦ��shader pass����Ⱦ�������
        DrawingSettings drawingSettings = new DrawingSettings(s_unlitShaderTag, sortingSettings);
        // ��Ⱦ����Ĺ�������
        FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
        // ��Ⱦ��͸������
        m_context.DrawRenderers(m_cullingResults, ref drawingSettings, ref filteringSettings);
        // ��Ⱦ��պ�
        m_context.DrawSkybox(m_camera);
        // ������Ⱦ�������Ϊ��͸���������򣬿�ʼ��Ⱦ��͸�����塣
        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;
        // ��Ⱦ͸������
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
