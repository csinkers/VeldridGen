using Microsoft.CodeAnalysis;

namespace VeldridGen.Symbols;

public class AttributeSymbols(Compilation compilation)
{
    // Attributes
    public INamedTypeSymbol ColorAttachment { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.ColorAttachmentAttribute");
    public INamedTypeSymbol DepthAttachment { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.DepthAttachmentAttribute");
    public INamedTypeSymbol FragmentShader { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.FragmentShaderAttribute");
    public INamedTypeSymbol Input { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.InputAttribute");
    public INamedTypeSymbol Name { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.NameAttribute");
    public INamedTypeSymbol Output { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.OutputAttribute");
    public INamedTypeSymbol Texture { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.TextureAttribute");
    public INamedTypeSymbol TextureArray { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.TextureArrayAttribute");
    public INamedTypeSymbol Sampler { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.SamplerAttribute");
    public INamedTypeSymbol UniformBuffer { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.UniformBufferAttribute");
    public INamedTypeSymbol StructuredBuffer { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.StructuredBufferAttribute");
    public INamedTypeSymbol ResourceSet { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.ResourceSetAttribute");
    public INamedTypeSymbol Uniform { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.UniformAttribute");
    public INamedTypeSymbol Structured { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.StructuredAttribute");
    public INamedTypeSymbol Vertex { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.VertexAttribute");
    public INamedTypeSymbol VertexShader { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.VertexShaderAttribute");
}
