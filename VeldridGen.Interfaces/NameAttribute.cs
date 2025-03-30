using System;

namespace VeldridGen.Interfaces;

public class NameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}
