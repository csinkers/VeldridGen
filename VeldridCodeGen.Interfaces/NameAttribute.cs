using System;

namespace VeldridCodeGen.Interfaces
{
    public class NameAttribute : Attribute
    {
        public NameAttribute(string name) => Name = name;
        public string Name { get; }
    }
}