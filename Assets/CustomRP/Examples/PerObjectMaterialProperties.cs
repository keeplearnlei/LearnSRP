using UnityEngine;

[DisallowMultipleComponent]
public class PerObjectMaterialProperties : MonoBehaviour
{
    [SerializeField] private Color color = Color.white;

    private static int baseColor = Shader.PropertyToID("_BaseColor");

    private static MaterialPropertyBlock block = null;

    private void Awake()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        if (block == null)
            block = new MaterialPropertyBlock();

        block.SetColor(baseColor, color);
        
        var m_renderer = GetComponent<Renderer>();
        m_renderer.SetPropertyBlock(block);
    }
}
