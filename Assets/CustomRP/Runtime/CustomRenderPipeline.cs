using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
    private CameraRenderer m_cameraRenderer = new CameraRenderer();
    private bool m_useDynamicBatching = false;
    private bool m_useGPUInstancing = false;

    public CustomRenderPipeline(bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher)
    {
        m_useDynamicBatching = useDynamicBatching;
        m_useGPUInstancing = useGPUInstancing;
        GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
    }

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach(var camera in cameras)
        {
            m_cameraRenderer.Render(context, camera, m_useDynamicBatching, m_useGPUInstancing);
        }
    }
}
