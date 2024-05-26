using System;

namespace VeldridGen.Interfaces;

public class FragmentShaderAttribute : Attribute
{
    public FragmentShaderAttribute(Type type) => Type = type ?? throw new ArgumentNullException(nameof(type));
    public Type Type { get; }
}