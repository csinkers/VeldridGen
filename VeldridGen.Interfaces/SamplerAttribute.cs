using System;
using Veldrid;

namespace VeldridGen.Interfaces
{
    public sealed class SamplerAttribute : Attribute
    {
        public SamplerAttribute(string name, ShaderStages stages = ShaderStages.Vertex | ShaderStages.Fragment)
        {
            Name = name;
            Stages = stages;
        }

        public string Name { get; }
        public ShaderStages Stages { get; }
    }
}