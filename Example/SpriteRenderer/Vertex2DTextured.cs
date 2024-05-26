using System.Numerics;
using VeldridGen.Interfaces;

namespace VeldridGen.Example.SpriteRenderer
{
#pragma warning disable CA1815 // Override equals and operator equals on value types
    public readonly partial struct Vertex2DTextured(float x, float y, float u, float v)
        : IVertexFormat
    {
        [Vertex("Position")] public Vector2 Position { get; } = new(x, y);
        [Vertex("TexCoords")] public Vector2 Texture { get; } = new(u, v);
    }
#pragma warning restore CA1815 // Override equals and operator equals on value types
}