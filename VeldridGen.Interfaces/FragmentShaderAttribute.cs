using System;

namespace VeldridGen.Interfaces;

public class FragmentShaderAttribute(Type type) : Attribute
{
    public Type Type { get; } = type ?? throw new ArgumentNullException(nameof(type));
}
