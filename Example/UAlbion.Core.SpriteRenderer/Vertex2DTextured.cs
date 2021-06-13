using System.Numerics;
using VeldridCodeGen.Interfaces;

namespace UAlbion.Core.SpriteRenderer
{
    public readonly partial struct Vertex2DTextured : IVertexFormat
    {
        [InputParam("vPosition")] public Vector2 Position { get; }
        [InputParam("vTextCoords")] public Vector2 Texture { get; }

        public Vertex2DTextured(float x, float y, float u, float v)
        {
            Position = new Vector2(x, y);
            Texture = new Vector2(u, v);
        }
    }
}
