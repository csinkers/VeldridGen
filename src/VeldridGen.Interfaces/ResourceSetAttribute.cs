using System;

namespace VeldridGen.Interfaces;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class ResourceSetAttribute(int order, Type type) : Attribute
{
    public override object TypeId => this;
    public int Order { get; } = order;
    public Type Type { get; } = type;
}
