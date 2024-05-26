using System;
using Veldrid;

namespace VeldridGen.Interfaces;

public sealed class TextureAttribute : Attribute
{
    public TextureAttribute(string name)
    {
        Name = name;
        IsReadOnly = true;
        Stages = ShaderStages.Vertex | ShaderStages.Fragment;
    }
    public TextureAttribute(string name, ShaderStages stages = ShaderStages.Vertex | ShaderStages.Fragment)
    {
        Name = name;
        IsReadOnly = true;
        Stages = stages;
    }
    public TextureAttribute(string name, bool isReadOnly = true, ShaderStages stages = ShaderStages.Vertex | ShaderStages.Fragment)
    {
        Name = name;
        IsReadOnly = isReadOnly;
        Stages = stages;
    }

    public string Name { get; }
    public bool IsReadOnly { get; }
    public ShaderStages Stages { get; }
}
