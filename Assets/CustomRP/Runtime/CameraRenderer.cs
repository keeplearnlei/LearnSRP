using System;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraRenderer
{
    private const string BufferName = "RenderCamera";
    private CommandBuffer m_buffer = new CommandBuffer() { name = BufferName};
    private ScriptableRenderContext m_context;
    // 存储剔除后的结果
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
        Submit();
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
        m_context.DrawSkybox(m_camera);
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

    private bool Cull()
    { 
        if (m_camera.TryGetCullingParameters(out var cullingParams))
        {
            m_cullingResults = m_context.Cull(ref cullingParams);
            return true;
        }
        return false;
    }
}
