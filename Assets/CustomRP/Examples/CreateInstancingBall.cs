using UnityEngine;

[DisallowMultipleComponent]
public class CreateInstancingBall : MonoBehaviour
{
    [SerializeField] private Material material;
    [SerializeField] private Mesh mesh;
    private int baseColor = Shader.PropertyToID("_BaseColor");

    private MaterialPropertyBlock block = null;
    private Matrix4x4[] matrixs = new Matrix4x4[1023];
    private Vector4[] colors = new Vector4[1023];

    private void Awake()
    {
        for (int i = 0; i < matrixs.Length; i++)
        {
            var pos = Random.insideUnitSphere * 10f;
            var rotation = Quaternion.Euler(Random.value * 360f, Random.value * 360f, Random.value * 360f);
            var scale = Vector3.one * Random.Range(0.5f, 1.5f);
            matrixs[i] = Matrix4x4.TRS(pos, rotation, scale);
            var color = new Vector4(Random.value, Random.value, Random.value, Random.Range(0.5f, 1.5f));
            colors[i] = color;
        }
    }

    private void Update()
    {
        if (block == null)
        { 
            block = new MaterialPropertyBlock();
            block.SetVectorArray(baseColor, colors);
        }

        Graphics.DrawMeshInstanced(mesh, 0, material, matrixs, 1023, block);
    }
}
