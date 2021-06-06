using System;

namespace UAlbion.Core
{
    public class VertexShaderAttribute : Attribute
    {
        public Type Type { get; }
        public VertexShaderAttribute(Type type) => Type = type ?? throw new ArgumentNullException(nameof(type));
    }
}