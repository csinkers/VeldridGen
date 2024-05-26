using System;

namespace VeldridGen.Interfaces;

public class VertexShaderAttribute : Attribute
{
    public VertexShaderAttribute(Type type) => Type = type ?? throw new ArgumentNullException(nameof(type));
    public Type Type { get; }
}
