using System;

namespace UAlbion.Core
{
    public class FragmentShaderAttribute : Attribute
    {
        public Type Type { get; }
        public FragmentShaderAttribute(Type type) => Type = type ?? throw new ArgumentNullException(nameof(type));
    }
}