using System.Numerics;
using VeldridCodeGen.Interfaces;

namespace UAlbion.Core.Veldrid
{
    public struct ViewMatrix : IUniformFormat
    {
        public ViewMatrix(Matrix4x4 matrix) => Matrix = matrix;
        [Uniform("uView")] public Matrix4x4 Matrix { get; }
    }
}