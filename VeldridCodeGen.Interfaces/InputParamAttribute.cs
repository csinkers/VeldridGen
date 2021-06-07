using System;

namespace VeldridCodeGen.Interfaces
{
    public sealed class InputParamAttribute : Attribute
    {
        public InputParamAttribute(string name) => Name = name;

        public string Name { get; }
    }
}