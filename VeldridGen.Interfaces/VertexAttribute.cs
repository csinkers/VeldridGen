using System;
using Veldrid;

namespace VeldridGen.Interfaces;

public sealed class VertexAttribute : Attribute
{
    /// <summary>
    /// Declares a vertex attribute with the given name.
    /// Note: The format will be set by the generator based on the type of the field or property.
    /// </summary>
    /// <param name="name">The name to use in the shader (will be prefixed with i or o, depending on if it's an input or output)</param>
    public VertexAttribute(string name)
    {
        Name = name;
        Format = VertexElementFormat.Float1; // Note: this will be replaced with an appropriate type by the generator
    }

    /// <summary>
    /// Declares a vertex attribute with the given name and format.
    /// </summary>
    /// <param name="name">The name to use in the shader (will be prefixed with i or o, depending on if it's an input or output)</param>
    /// <param name="format">The vertex element format to use</param>
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
