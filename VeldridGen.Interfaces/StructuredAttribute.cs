using System;

namespace VeldridGen.Interfaces;

public sealed class StructuredAttribute(string name) : Attribute
{
    public string Name { get; } = name;
    public string EnumPrefix { get; set; }
}
