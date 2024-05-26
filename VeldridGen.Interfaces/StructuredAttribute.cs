using System;

namespace VeldridGen.Interfaces;

public sealed class StructuredAttribute : Attribute
{
    public StructuredAttribute(string name) => Name = name;
    public string Name { get; }
    public string EnumPrefix { get; set; }
}
