﻿using Veldrid;
namespace UAlbion.Core.SpriteRenderer
{
    public partial struct SpriteIntermediateData
    {
        public static VertexLayoutDescription Layout = new(
            new VertexElementDescription("TexPosition", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
            new VertexElementDescription("Layer", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float1),
            new VertexElementDescription("Flags", VertexElementSemantic.TextureCoordinate, VertexElementFormat.UInt1),
            new VertexElementDescription("NormCoords", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
            new VertexElementDescription("WorldPosition", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float3));
    }
}