using System;
using System.Numerics;
using UAlbion.CodeGen;
using Veldrid;

namespace UAlbion.Core
{
#pragma warning disable CA1051 // Do not declare visible instance fields
    public readonly partial struct Vertex2DTextured : IEquatable<Vertex2DTextured>, IVertexFormat
    {
        [InputParam("vPosition")] public Vector2 Position { get; }
        [InputParam("vTextCoords")] public Vector2 Texture { get; }

        public Vertex2DTextured(Vector2 position, Vector2 textureCoordinates)
        {
            Position = position;
            Texture = textureCoordinates;
        }

        public Vertex2DTextured(float x, float y, float u, float v)
        {
            Position = new Vector2(x, y);
            Texture = new Vector2(u, v);
        }
    }

    // To be generated
    public readonly partial struct Vertex2DTextured
    {
        public static VertexLayoutDescription Layout = new(
            new VertexElementDescription("vPosition", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
            new VertexElementDescription("vTextCoords", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2));

        public override bool Equals(object obj) => obj is Vertex2DTextured other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Position, Texture);
        public static bool operator ==(Vertex2DTextured left, Vertex2DTextured right) => left.Equals(right);
        public static bool operator !=(Vertex2DTextured left, Vertex2DTextured right) => !(left == right);
        public bool Equals(Vertex2DTextured other) => Position == other.Position && Texture == other.Texture;
    }

#pragma warning restore CA1051 // Do not declare visible instance fields
}
