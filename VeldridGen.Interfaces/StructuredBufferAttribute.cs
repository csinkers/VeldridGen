using System;
using Veldrid;

namespace VeldridGen.Interfaces
{
    public sealed class StructuredBufferAttribute : Attribute
    {
        public StructuredBufferAttribute(string name, ShaderStages stages) : this(name, true, stages) { }
        public StructuredBufferAttribute(string name, bool isReadOnly = true, ShaderStages stages = ShaderStages.Vertex | ShaderStages.Fragment)
        {
            Name = name;
            IsReadOnly = isReadOnly;
            Stages = stages;
        }

        public string Name { get; }
        public bool IsReadOnly { get; }
        public ShaderStages Stages { get; }
    }
}