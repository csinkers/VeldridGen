using System;

namespace VeldridGen.Interfaces;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class InputAttribute : Attribute
{
    public override object TypeId => this;
    public int Order { get; }
    public Type Type { get; }
    public int InstanceStep { get; set; }
    public InputAttribute(int order, Type type)
    {
        Order = order;
        Type = type ?? throw new ArgumentNullException(nameof(type));
    }
}
