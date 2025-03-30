using System;
using Veldrid;

namespace VeldridGen.Interfaces;

public sealed class SamplerAttribute(string name, ShaderStages stages = ShaderStages.Vertex | ShaderStages.Fragment)
    : Attribute
{
    public string Name { get; } = name;
    public ShaderStages Stages { get; } = stages;
}
