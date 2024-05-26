using System.Numerics;
using System.Runtime.InteropServices;
using Veldrid;
using VeldridGen.Example.Engine;
using VeldridGen.Interfaces;

#pragma warning disable CA1051 // Do not declare visible instance fields
#pragma warning disable CA1815 // Override equals and operator equals on value types
namespace VeldridGen.Example.SpriteRenderer
{
    public sealed partial class CommonSet : ResourceSetHolder
    {
        [UniformBuffer("_Shared")]                          SingleBuffer<GlobalInfo>       _globalInfo; 
        [UniformBuffer("_Projection", ShaderStages.Vertex)] SingleBuffer<ProjectionMatrix> _projection; 
        [UniformBuffer("_View",       ShaderStages.Vertex)] SingleBuffer<ViewMatrix>       _view; 
        [Texture("uPalette",          ShaderStages.Fragment)] ITextureHolder               _palette;
    }

    [StructLayout(LayoutKind.Sequential)]
    public partial struct GlobalInfo : IUniformFormat
    {
        [Uniform("uWorldSpacePosition")] public Vector3 WorldSpacePosition;
        [Uniform("_globalInfo_pad1")] readonly uint _padding1;

        [Uniform("uCameraLookDirection")] public Vector2 CameraDirection;
        [Uniform("uResolution")] public Vector2 Resolution;

        [Uniform("uTime")] public float Time;
        [Uniform("uEngineFlags", EnumPrefix = "EF")] public EngineFlags EngineFlags;
        [Uniform("_global_pad1")] readonly uint _pad1;
        [Uniform("_global_pad2")] readonly uint _pad2;
    }

    public partial struct ProjectionMatrix : IUniformFormat
    {
        public ProjectionMatrix(Matrix4x4 matrix) => Matrix = matrix;
        [Uniform("uProjection")] public Matrix4x4 Matrix { get; }
    }

    public partial struct ViewMatrix : IUniformFormat
    {
        public ViewMatrix(Matrix4x4 matrix) => Matrix = matrix;
        [Uniform("uView")] public Matrix4x4 Matrix { get; }
    }
}
#pragma warning restore CA1051 // Do not declare visible instance fields
#pragma warning restore CA1815 // Override equals and operator equals on value types
