using Veldrid;
namespace VeldridGen.Example.SpriteRenderer
{
    internal partial struct ColorOnly
    {
        public static VertexLayoutDescription GetLayout(bool input) => new(
            new VertexElementDescription((input ? "i" : "o") + "Color", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4));
    }
}
