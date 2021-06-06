using System.Numerics;
using UAlbion.CodeGen;

namespace UAlbion.Core
{
    public struct ProjectionMatrix : IUniformFormat
    {
        public ProjectionMatrix(Matrix4x4 matrix) => Matrix = matrix;
        [Uniform("uProjection")] public Matrix4x4 Matrix { get; }
    }
}