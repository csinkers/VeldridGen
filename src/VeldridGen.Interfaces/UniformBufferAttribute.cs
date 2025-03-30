using System;
using Veldrid;

namespace VeldridGen.Interfaces;

public sealed class UniformBufferAttribute(
    string name,
    ShaderStages stages = ShaderStages.Fragment | ShaderStages.Vertex)
    : Attribute
{
    public string Name { get; } = name;
    public ShaderStages Stages { get; } = stages;
}
