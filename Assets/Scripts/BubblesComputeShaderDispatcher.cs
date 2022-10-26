using UnityEngine;

public class BubblesComputeShaderDispatcher : BaseComputeShaderDispatcher
{
    private const int THREAD_GROUP_COUNT = 1;

    public bool DispatchOnUpdate = false;
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
        // paint the background with 32x32x1 x 8x1x1 threads
        DispatchShader(_kernelIndexes[0], 32, 32);
        // paint the bubbles with 1x1x1 thread groups (total threads will depend on numthreads(x,y,z))
        DispatchShader(_kernelIndexes[1], THREAD_GROUP_COUNT, THREAD_GROUP_COUNT);
    }

    public void OnValidate()
    {
        if (_isInitialized)
        {
            DispatchShaders();
        }
    }

    void Update()
    {
        if (DispatchOnUpdate)
        {
            computeShader.SetFloat("Time", Time.time);
            DispatchShaders();
        }
    }
}
