using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
    private CameraRenderer m_cameraRenderer = new CameraRenderer();

    public CustomRenderPipeline()
    { 
        GraphicsSettings.useScriptableRenderPipelineBatching = false;
    }

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        foreach(var camera in cameras)
        {
            m_cameraRenderer.Render(context, camera);
        }
    }
}
