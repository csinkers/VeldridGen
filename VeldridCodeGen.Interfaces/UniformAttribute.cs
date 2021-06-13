using System;

namespace VeldridCodeGen.Interfaces
{
    public sealed class UniformAttribute : Attribute
    {
        public UniformAttribute(string name) => Name = name;
        public string Name { get; }
        public string EnumPrefix { get; set; }
    }
}