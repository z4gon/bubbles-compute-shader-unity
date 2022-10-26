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

        InitBubblesBuffer();

        _isInitialized = true;
    }

    private void InitBubblesBuffer()
    {
        int kernelIndex = computeShader.FindKernel(KernelName.MovingBubbles.ToString());

        uint threadGroupSizeX;

        // get the thread groups size in the x dimension, we don't care about y and z becasuse numthreads is 8x1x1
        computeShader.GetKernelThreadGroupSizes(kernelIndex, out threadGroupSizeX, out _, out _);

        // 32x1x1 x 8x1x1 = 32x8
        int amountOfBubbles = ThreadGroupsCount * (int)threadGroupSizeX;

        // create the array of bubbles with random values
        _bubbles = Bubbles.CreateBubbles(amountOfBubbles, TEXTURE_RESOLUTION, TEXTURE_RESOLUTION);

        // each Vector2 is two floats, total 5 floats per struct
        int memorySizeOfBubble = (2 + 2 + 1) * sizeof(float);

        // initialize the compute buffer
        _bubblesBuffer = new ComputeBuffer(_bubbles.Length, amountOfBubbles * memorySizeOfBubble);

        // fill with the data
        _bubblesBuffer.SetData(_bubbles);

        // set the buffer to the compute shader
        computeShader.SetBuffer(kernelIndex, "BubblesBuffer", _bubblesBuffer);
    }

    private void DispatchShaders()
    {
        // paint the background with 32x32x1 x 8x1x1 threads
        DispatchShader(_kernelIndexes[0], 32, 32);
        // paint the bubbles with 1x1x1 thread groups (total threads will depend on numthreads(x,y,z))
        DispatchShader(_kernelIndexes[1], ThreadGroupsCount);
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
            computeShader.SetFloat("DeltaTime", Time.deltaTime);
            DispatchShaders();
        }
    }
}
