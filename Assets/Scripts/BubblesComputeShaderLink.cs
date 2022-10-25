using UnityEngine;

public class BubblesComputeShaderLink : BaseComputeShaderLink
{
    private const int THREAD_GROUP_COUNT = 1;

    public Vector3 CirclePositionAndRadius = new Vector3(0, 0, 64);
    public Color CircleColor = new Color(1, 0, 0, 1);

    protected override void Start()
    {
        base.Start();
        DispatchShader(_kernelIndexes[0], THREAD_GROUP_COUNT, THREAD_GROUP_COUNT);
    }

    protected override void InitShader()
    {
        base.InitShader();

        computeShader.SetVector("CirclePositionAndRadius", CirclePositionAndRadius);
        computeShader.SetVector("CircleColor", CircleColor);

        _isInitialized = true;
    }

    public void OnValidate()
    {
        // Debug.Log($"TextureResolution: {TextureResolution}");
        // Debug.Log($"ThreadGroupsX: {ThreadGroupsX}");
        // Debug.Log($"ThreadGroupsY: {ThreadGroupsY}");

        if (_isInitialized)
        {
            DispatchShader(_kernelIndexes[0], THREAD_GROUP_COUNT, THREAD_GROUP_COUNT);
        }
    }
}
