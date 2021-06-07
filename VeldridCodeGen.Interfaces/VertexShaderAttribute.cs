using System;

namespace VeldridCodeGen.Interfaces
{
    public class VertexShaderAttribute : Attribute
    {
        public Type Type { get; }
        public VertexShaderAttribute(Type type) => Type = type ?? throw new ArgumentNullException(nameof(type));
    }
}