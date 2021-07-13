using Veldrid;
namespace VeldridGen.Example.SpriteRenderer
{
    public partial struct Vertex2DTextured
    {
        public static VertexLayoutDescription Layout { get; } = new(
            new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
            new VertexElementDescription("TexCoords", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2));
    }
}
