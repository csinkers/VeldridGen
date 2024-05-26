using System;

namespace VeldridGen.Interfaces;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class ResourceSetAttribute : Attribute
{
    public override object TypeId => this;
    public int Order { get; }
    public Type Type { get; }
    public ResourceSetAttribute(int order, Type type)
    {
        Order = order;
        Type = type;
    }
}
