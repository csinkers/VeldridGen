using System;

namespace VeldridGen.Interfaces;

/// <summary>
/// Applied to a type derived from <see cref="IPipelineHolder"/> to indicate which fragment shader it uses.
/// </summary>
/// <param name="type">The type for the fragment shader, deriving from <see cref="IFragmentShader"/></param>
public class FragmentShaderAttribute(Type type) : Attribute
{
    public Type Type { get; } = type ?? throw new ArgumentNullException(nameof(type));
}
