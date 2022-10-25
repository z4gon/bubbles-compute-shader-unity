# Basic Compute Shaders

Written in HLSL in **Unity 2021.3.10f1**

### References

- [Compute Shaders course by Nik Lever](https://www.udemy.com/course/compute-shaders)

## Sections

- [Definition of the Compute Shader](#definition-of-the-compute-shader)
- [Dispatching the Compute Shader](#dispatching-the-compute-shader)
  - [Render Texture](#render-texture)
  - [Renderer](#renderer)
  - [Kernel Index](#kernel-index)
  - [Texture Assignment](#texture-assignment)
  - [Dispatching](#dispatching)

## Definition of the Compute Shader

- The **#pragma** declaration lets the compiler know the function name associated with the kernel definition.
- A Texture 2D buffer is defined as the result of the computation by declaring the `RWTexture2D<float4> Result`.
- `[numthreads(8,8,1)]` specifies that the thread group will have 8x8x1 number of threads, each working on a pixel on parallel.
  - We use 1 for the z dimension, because we don't need 3D depth.
  - 8x8 will be the size of the square of pixels handled by this thread group.
  - On a **256x256** pixel texture, we need **32x32x1** thread groups, each covering **8x8x1** pixels. All this will be parallelized.

```hlsl
// Each #kernel tells which function to compile; you can have many kernels
#pragma kernel CSMain

// Create a RenderTexture with enableRandomWrite flag and set it
// with cs.SetTexture
RWTexture2D<float4> Result;

[numthreads(8,8,1)]
void CSMain (uint3 id : SV_DispatchThreadID)
{
    Result[id.xy] = float4(1, 0, 0, 0);
}
```

## Dispatching the Compute Shader

#### Render Texture

- Create a **Render Texture** object, which will act as the output of the **Compute Shader** computation.

```cs
int TEXTURE_RESOLUTION = 256;

// initialize the texture with just x and y dimensions, it could have z depth
_renderTexture = new RenderTexture(TEXTURE_RESOLUTION, TEXTURE_RESOLUTION, 0);

// allow the compute shader to write to the texture
_renderTexture.enableRandomWrite = true;

// RenderTexture constructor does not actually create the hardware texture
// https://docs.unity3d.com/ScriptReference/RenderTexture.Create.html
_renderTexture.Create();
```

#### Renderer

- Get a hold of the current **Renderer** of the game object's mesh.

```cs
_renderer = GetComponent<Renderer>();
_renderer.enabled = true;
```

#### Kernel Index

- Get a reference to the **kernel** id from the **Compute Shader**, which corresponds to the main function being executed.

```cs
// get a reference to the kernel defined in the #pragma inside the compute shader
_kernelIndex = computeShader.FindKernel("CSMain");
```

#### Texture Assignment

- Assign the **Render Texture** to the **Compute Shader**, so that it can write to it.
- Assign the **Render Texture** to the **Material** in the renderer, so it can use it.

```cs
// set the texture to the compute shader, so it can write to it
computeShader.SetTexture(_kernelIndex, "Result", _renderTexture);

// set the texture to the material, so it can use the texture
_renderer.material.SetTexture("_MainTex", _renderTexture);
```

#### Dispatching

- Our compute shader has **numthreads(8,8,1)** per each thread group.
- That means it will compute a square of 8x8x1 pixels or threads, per each thread group.
- **Dispatch()** takes in the amount of thread groups it should have in all 3 dimensions.
- 32 thread groups means 32 \* 8 = 256 pixels, that covers the whole texture.

```cs
// dispatches the kernel with the amount of thread groups = x * y * 1
// we keep z = 1 because we are working on a 2D texture, no need for depth
computeShader.Dispatch(_kernelIndex, x, y, 1);
```

https://user-images.githubusercontent.com/4588601/197791116-834b7201-975e-4280-89fb-c26432eb9b5b.mp4




