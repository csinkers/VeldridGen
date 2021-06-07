using System.Numerics;
using VeldridCodeGen.Interfaces;

namespace UAlbion.Core.SpriteRenderer
{
    public struct ProjectionMatrix : IUniformFormat
    {
        public ProjectionMatrix(Matrix4x4 matrix) => Matrix = matrix;
        [Uniform("uProjection")] public Matrix4x4 Matrix { get; }
    }
}