using UnityEngine;

public enum KernelName
{
    SolidRed,
    SolidYellow,
    SplitScreen
}

public class AssignTexture : MonoBehaviour
{
    private const int TEXTURE_RESOLUTION = 256;

    public ComputeShader computeShader;
    public KernelName KernelName = KernelName.SolidRed;

    // our compute shader has numthreads(8,8,1) per each thread group
    // 32 thread groups means 32 * 8 = 256 pixels, that covers the whole texture

    [Range(0, 32)]
    public int ThreadGroupsX = 16;

    [Range(0, 32)]
    public int ThreadGroupsY = 16;

    private Renderer _renderer;
    private RenderTexture _renderTexture;

    private int _kernelIndex;
    private bool _isInitialized = false;

    void Start()
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

        DispatchShader(ThreadGroupsX, ThreadGroupsY);
    }

    private void InitShader()
    {
        // get a reference to the kernel defined in the #pragma inside the compute shader
        _kernelIndex = computeShader.FindKernel(KernelName.ToString());

        // set the texture to the compute shader, so it can write to it
        computeShader.SetTexture(_kernelIndex, "Result", _renderTexture);

        // set the texture to the material, so it can use the texture
        _renderer.material.SetTexture("_MainTex", _renderTexture);

        _isInitialized = true;
    }

    // dispatches the kernel with the amount of thread groups = x * y * 1
    // we keep z = 1 because we are working on a 2D texture, no need for depth
    private void DispatchShader(int x, int y) => computeShader.Dispatch(_kernelIndex, x, y, 1);

    public void OnValidate()
    {
        // Debug.Log($"TextureResolution: {TextureResolution}");
        // Debug.Log($"ThreadGroupsX: {ThreadGroupsX}");
        // Debug.Log($"ThreadGroupsY: {ThreadGroupsY}");

        if (_isInitialized)
        {
            DispatchShader(ThreadGroupsX, ThreadGroupsY);
        }
    }
}
