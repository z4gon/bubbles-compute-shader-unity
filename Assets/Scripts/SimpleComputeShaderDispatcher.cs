using UnityEngine;

public class SimpleComputeShaderDispatcher : BaseComputeShaderDispatcher
{
    // our compute shader has numthreads(8,8,1) per each thread group
    // 32 thread groups means 32 * 8 = 256 pixels, that covers the whole texture

    [Range(0, 32)]
    public int ThreadGroupsX = 16;

    [Range(0, 32)]
    public int ThreadGroupsY = 16;

    public Vector3 CirclePositionAndRadius = new Vector3(0, 0, 64);
    public Vector4 RectPositionAndSize = new Vector4(0, 0, 64, 64);

    protected override void Start()
    {
        base.Start();
        DispatchShader(_kernelIndexes[0], ThreadGroupsX, ThreadGroupsY);
    }

    protected override void InitShader()
    {
        base.InitShader();

        computeShader.SetVector("CirclePositionAndRadius", CirclePositionAndRadius);
        computeShader.SetVector("RectPositionAndSize", RectPositionAndSize);

        _isInitialized = true;
    }

    public void OnValidate()
    {
        // Debug.Log($"TextureResolution: {TextureResolution}");
        // Debug.Log($"ThreadGroupsX: {ThreadGroupsX}");
        // Debug.Log($"ThreadGroupsY: {ThreadGroupsY}");

        if (_isInitialized)
        {
            DispatchShader(_kernelIndexes[0], ThreadGroupsX, ThreadGroupsY);
        }
    }
}
