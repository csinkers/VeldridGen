﻿using Veldrid;
namespace UAlbion.Core.SpriteRenderer
{
    public partial struct Vertex2DTextured
    {
        public static VertexLayoutDescription Layout = new(
            new VertexElementDescription("Position", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
            new VertexElementDescription("TexCoords", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2));
    }
}