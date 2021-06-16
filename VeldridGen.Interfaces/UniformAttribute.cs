using System;

namespace VeldridGen.Interfaces
{
    public sealed class UniformAttribute : Attribute
    {
        public UniformAttribute(string name) => Name = name;
        public string Name { get; }
        public string EnumPrefix { get; set; }
    }
}
