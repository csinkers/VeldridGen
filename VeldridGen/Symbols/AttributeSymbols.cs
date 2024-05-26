using Microsoft.CodeAnalysis;

namespace VeldridGen.Symbols;

public class AttributeSymbols
{
    // Attributes
    public INamedTypeSymbol ColorAttachment { get; }
    public INamedTypeSymbol DepthAttachment { get; }
    public INamedTypeSymbol FragmentShader { get; }
    public INamedTypeSymbol Input { get; }
    public INamedTypeSymbol Name { get; }
    public INamedTypeSymbol Output { get; }
    public INamedTypeSymbol Texture { get; }
    public INamedTypeSymbol TextureArray { get; }
    public INamedTypeSymbol Sampler { get; }
    public INamedTypeSymbol UniformBuffer { get; }
    public INamedTypeSymbol StructuredBuffer { get; }
    public INamedTypeSymbol ResourceSet { get; }
    public INamedTypeSymbol Uniform { get; }
    public INamedTypeSymbol Structured { get; }
    public INamedTypeSymbol Vertex { get; }
    public INamedTypeSymbol VertexShader { get; }


    public AttributeSymbols(Compilation compilation)
    {
        ColorAttachment = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.ColorAttachmentAttribute");
        DepthAttachment = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.DepthAttachmentAttribute");
        Vertex = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.VertexAttribute");
        Texture = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.TextureAttribute");
        TextureArray = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.TextureArrayAttribute");
        Sampler = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.SamplerAttribute");
        UniformBuffer = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.UniformBufferAttribute");
        StructuredBuffer = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.StructuredBufferAttribute");

        FragmentShader = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.FragmentShaderAttribute");
        Input = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.InputAttribute");
        Name = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.NameAttribute");
        Output = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.OutputAttribute");
        ResourceSet = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.ResourceSetAttribute");
        Uniform = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.UniformAttribute");
        Structured = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.StructuredAttribute");
        VertexShader = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.VertexShaderAttribute");
    }
}