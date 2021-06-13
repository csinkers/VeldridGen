#if false
using System;

namespace VeldridCodeGen.Interfaces
{
    public class FragmentShaderAttribute : Attribute
    {
        public Type Type { get; }
        public FragmentShaderAttribute(Type type) => Type = type ?? throw new ArgumentNullException(nameof(type));
    }
}
#endif