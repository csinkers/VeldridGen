using System;
using Veldrid;

namespace VeldridGen.Interfaces;

public sealed class VertexAttribute : Attribute
{
    public VertexAttribute(string name) => Name = name;
    public VertexAttribute(string name, VertexElementFormat format)
    {
        Name = name;
        Format = format;
    }

    public string Name { get; }
    public VertexElementFormat Format { get; }
    public bool Flat { get; set; }
    public string EnumPrefix { get; set; }
}
