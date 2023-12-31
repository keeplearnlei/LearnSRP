using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "Rendering/CreateRenderPipelineAsset", order = 1)]
public class CustomRenderPipelineAsset : RenderPipelineAsset
{
    [SerializeField] private bool useDynamicBatching = true;
    [SerializeField] bool useGPUInstancing = true;
    [SerializeField] bool useSRPBatcher = true;

    protected override RenderPipeline CreatePipeline()
    {
        return new CustomRenderPipeline(useDynamicBatching, useGPUInstancing, useSRPBatcher);
    }
}
