using System;
using Veldrid;

namespace VeldridGen.Interfaces
{
    public sealed class ResourceAttribute : Attribute
    {
        public ResourceAttribute(string name)
        {
            Name = name;
            Stages = ShaderStages.Fragment | ShaderStages.Vertex;
        }

        public ResourceAttribute(string name, ShaderStages stages)
        {
            Name = name;
            Stages = stages;
        }

        public string Name { get; }
        public ShaderStages Stages { get; }
    }
}
