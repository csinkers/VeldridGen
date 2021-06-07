using System;
using Veldrid;

namespace UAlbion.Core.SpriteRenderer
{
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
}
