using UnityEngine;

public class BubblesComputeShaderDispatcher : BaseComputeShaderDispatcher
{
    private const int THREAD_GROUP_COUNT = 1;

    public Vector3 CirclePositionAndRadius = new Vector3(0, 0, 64);
    public Color CircleColor = new Color(0, 1, 0, 1);
    public Color BackgroundColor = new Color(0, 0, 1, 1);

    protected override void Start()
    {
        base.Start();
        DispatchShaders();
    }

    protected override void InitShader()
    {
        base.InitShader();

        computeShader.SetVector("CirclePositionAndRadius", CirclePositionAndRadius);
        computeShader.SetVector("CircleColor", CircleColor);
        computeShader.SetVector("BackgroundColor", BackgroundColor);

        _isInitialized = true;
    }

    private void DispatchShaders()
    {
        DispatchShader(_kernelIndexes[0], 32, 32);
        DispatchShader(_kernelIndexes[1], THREAD_GROUP_COUNT, THREAD_GROUP_COUNT);
    }

    public void OnValidate()
    {
        if (_isInitialized)
        {
            DispatchShaders();
        }
    }
}
