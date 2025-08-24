using System;

namespace VeldridGen.Interfaces;

/// <summary>
/// Applied to shader types deriving from <see cref="IVertexShader"/> or <see cref="IFragmentShader"/> to declare
/// one of the output vertex formats for the shader (in the case of a vertex shader) or an output framebuffer-holder type (for a fragment shader).
/// </summary>
/// <param name="order">The output slot number</param>
/// <param name="type">The type of the output slots vertex format (deriving from <see cref="IVertexFormat"/>)/framebuffer (deriving from <see cref="IFramebufferHolder"/>)</param>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class OutputAttribute(int order, Type type) : Attribute
{
    public override object TypeId => this;
    public int Order { get; } = order;
    public Type Type { get; } = type ?? throw new ArgumentNullException(nameof(type));
}
