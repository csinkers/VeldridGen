using System.Numerics;
using UAlbion.CodeGen;

namespace UAlbion.Core
{
    public struct ViewMatrix : IUniformFormat
    {
        public ViewMatrix(Matrix4x4 matrix) => Matrix = matrix;
        [Uniform("uView")] public Matrix4x4 Matrix { get; }
    }
}