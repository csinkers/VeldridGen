using System;

namespace VeldridGen.Interfaces;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class OutputAttribute : Attribute
{
    public override object TypeId => this;
    public int Order { get; }
    public Type Type { get; }
    public OutputAttribute(int order, Type type)
    {
        Order = order;
        Type = type ?? throw new ArgumentNullException(nameof(type));
    }
}