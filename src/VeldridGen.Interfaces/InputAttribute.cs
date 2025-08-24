using System;

namespace VeldridGen.Interfaces;

/// <summary>
/// Applied to shader types deriving from <see cref="IVertexShader"/> or <see cref="IFragmentShader"/> to declare one of the input vertex formats for the shader.
/// </summary>
/// <param name="order">The input slot number</param>
/// <param name="type">The type of the input slots vertex format, deriving from <see cref="IVertexFormat"/></param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class InputAttribute(int order, Type type) : Attribute
{
    public override object TypeId => this;
    public int Order { get; } = order;
    public Type Type { get; } = type ?? throw new ArgumentNullException(nameof(type));
    public int InstanceStep { get; set; }
}
