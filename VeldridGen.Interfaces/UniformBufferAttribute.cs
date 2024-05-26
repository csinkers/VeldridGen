using System;
using Veldrid;

namespace VeldridGen.Interfaces;

public sealed class UniformBufferAttribute : Attribute
{
    public UniformBufferAttribute(string name, ShaderStages stages = ShaderStages.Fragment | ShaderStages.Vertex)
    {
        Name = name;
        Stages = stages;
    }

    public string Name { get; }
    public ShaderStages Stages { get; }
}
