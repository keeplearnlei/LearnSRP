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

    public void Render(ScriptableRenderContext context, Camera camera, bool useDynamicBatching, bool useGPUInstancing)
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
        
        DrawVisibleGeometry(useDynamicBatching, useGPUInstancing);
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
        // �����������
        m_context.SetupCameraProperties(m_camera);
        // ���������clearFlags��������Ⱦ������
        CameraClearFlags flag = m_camera.clearFlags;
        bool clearDepth = flag <= CameraClearFlags.Depth;
        // SkyBox�Ͳ��������ˣ����ջ���һ����պУ�Ҳ���Զ�������
        bool clearColor = flag == CameraClearFlags.Color;
        Color color = clearColor ? m_camera.backgroundColor.linear : Color.clear;
        m_buffer.ClearRenderTarget(clearDepth, clearColor, color);
        m_buffer.BeginSample(BufferName);
        ExecuteBuffer();
    }

    private void DrawVisibleGeometry(bool useDynamicBatching, bool useGPUInstancing)
    {
        // ������Ⱦ�������Ϊ��͸���������򣬿�ʼ��Ⱦ��͸�����塣
        SortingSettings sortingSettings = new SortingSettings()
        { 
            criteria = SortingCriteria.CommonOpaque,
        };
        // ������Ⱦ��shader pass����Ⱦ�������
        DrawingSettings drawingSettings = new DrawingSettings(s_unlitShaderTag, sortingSettings) 
        {
            enableDynamicBatching = useDynamicBatching,
            enableInstancing = useGPUInstancing,
        };

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
