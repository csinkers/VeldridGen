using System;
using System.Text;

namespace VeldridCodeGen
{
    static class VertexFormatGenerator
    {
        public static void Generate(StringBuilder sb, VeldridTypeInfo type)
        {
            throw new NotImplementedException();
        }
            /*
    public readonly partial struct Vertex2DTextured // match access specifier, name
    {
        public static VertexLayoutDescription Layout = new(
            new VertexElementDescription("vPosition", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2), // attrib.Name
            new VertexElementDescription("vTextCoords", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2));

            Types:
            float => Float1
            Vector2 => Float2
            Vector3 => Float3
            Vector4 => Float4
            uint => UInt1
            int => Int1
            // TODO: various packed formats, like half floats, 2xbyte, 4xbyte, 2xushort etc
    }
            Vertex GLSL:
// {type name}
layout(location = {index}) in vec2 {attrib.Name};
layout(location = {index}) in uint {attrib.Name};

             */
    }
}