using UnityEngine;

public class BaseComputeShaderDispatcher : MonoBehaviour
{
    protected const int TEXTURE_RESOLUTION = 256;

    public ComputeShader computeShader;

    public KernelName[] KernelNames;

    protected Renderer _renderer;
    protected RenderTexture _renderTexture;

    protected int[] _kernelIndexes;
    protected bool _isInitialized = false;

    protected virtual void Start()
    {
        // initialize the texture with just x and y dimensions, it could have z depth
        _renderTexture = new RenderTexture(TEXTURE_RESOLUTION, TEXTURE_RESOLUTION, 0);

        // allow the compute shader to write to the texture
        _renderTexture.enableRandomWrite = true;

        // RenderTexture constructor does not actually create the hardware texture
        // https://docs.unity3d.com/ScriptReference/RenderTexture.Create.html
        _renderTexture.Create();

        _renderer = GetComponent<Renderer>();
        _renderer.enabled = true;

        // initialize the shader
        InitShader();
    }

    protected virtual void InitShader()
    {
        _kernelIndexes = new int[KernelNames.Length];

        for (int i = 0; i < KernelNames.Length; i++)
        {
            // get a reference to the kernel defined in the #pragma inside the compute shader
            _kernelIndexes[i] = computeShader.FindKernel(KernelNames[i].ToString());
            // set the texture to the compute shader, so it can write to it
            computeShader.SetTexture(_kernelIndexes[i], "Result", _renderTexture);
        }

        computeShader.SetInt("TextureResolution", TEXTURE_RESOLUTION);

        // set the texture to the material, so it can use the texture
        _renderer.material.SetTexture("_MainTex", _renderTexture);
    }

    // dispatches the kernel with the amount of thread groups = x * y * 1
    // we keep z = 1 because we are working on a 2D texture, no need for depth
    protected void DispatchShader(int kernelIndex, int x, int y) => computeShader.Dispatch(kernelIndex, x, y, 1);
}
