using System;

namespace VeldridGen.Interfaces;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class InputAttribute(int order, Type type) : Attribute
{
    public override object TypeId => this;
    public int Order { get; } = order;
    public Type Type { get; } = type ?? throw new ArgumentNullException(nameof(type));
    public int InstanceStep { get; set; }
}
