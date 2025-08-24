using System;

namespace VeldridGen.Interfaces;

/// <summary>
/// Applied to shader types deriving from <see cref="IVertexShader"/> or <see cref="IFragmentShader"/> to indicate the filename of the shader.
/// </summary>
/// <param name="name">The filename of the shader</param>
public class NameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
