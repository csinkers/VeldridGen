﻿using Veldrid;
namespace VeldridGen.Example.SpriteRenderer
{
    internal partial struct ColorOnly
    {
        public static VertexLayoutDescription Layout { get; } = new(
            new VertexElementDescription("Color", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float4));
    }
}
