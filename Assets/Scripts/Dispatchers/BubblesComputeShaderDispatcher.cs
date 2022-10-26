using UnityEngine;

public class BubblesComputeShaderDispatcher : BaseComputeShaderDispatcher
{
    public bool DispatchOnUpdate = false;
    public int ThreadGroupsCount = 1;
    public Vector3 CirclePositionAndRadius = new Vector3(0, 0, 64);
    public Color CircleColor = new Color(0, 1, 0, 1);
    public Color BackgroundColor = new Color(0, 0, 1, 1);

    private Bubble[] _bubbles;
    private ComputeBuffer _bubblesBuffer;


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

    private void InitBubbles()
    {
        int kernelIndex = computeShader.FindKernel(KernelName.MovingBubbles.ToString());

        uint threadGroupSizeX;

        // get the thread groups size in the x dimension, we don't care about y and z
        computeShader.GetKernelThreadGroupSizes(kernelIndex, out threadGroupSizeX, out _, out _);

        // int amountOfBubbles =
    }

    private void DispatchShaders()
    {
        // paint the background with 32x32x1 x 8x1x1 threads
        DispatchShader(_kernelIndexes[0], 32, 32);
        // paint the bubbles with 1x1x1 thread groups (total threads will depend on numthreads(x,y,z))
        DispatchShader(_kernelIndexes[1], ThreadGroupsCount, ThreadGroupsCount);
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
