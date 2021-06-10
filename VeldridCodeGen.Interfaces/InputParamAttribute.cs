using System;
using Veldrid;

namespace VeldridCodeGen.Interfaces
{
    public sealed class InputParamAttribute : Attribute
    {
        public InputParamAttribute(string name) => Name = name;
        public InputParamAttribute(string name, VertexElementFormat format)
        {
            Name = name;
            Format = format;
        }

        public string Name { get; }
        public VertexElementFormat Format { get; }
    }
}