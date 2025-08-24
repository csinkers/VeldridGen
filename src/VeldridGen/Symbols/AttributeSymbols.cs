using Microsoft.CodeAnalysis;

namespace VeldridGen.Symbols;

public class AttributeSymbols(Compilation compilation)
{
    public const string Namespace = "VeldridGen.Interfaces";
    public const string ColorAttachmentAttributeName  = Namespace + ".ColorAttachmentAttribute";
    public const string DepthAttachmentAttributeName  = Namespace + ".DepthAttachmentAttribute";
    public const string FragmentShaderAttributeName   = Namespace + ".FragmentShaderAttribute";
    public const string InputAttributeName            = Namespace + ".InputAttribute";
    public const string NameAttributeName             = Namespace + ".NameAttribute";
    public const string OutputAttributeName           = Namespace + ".OutputAttribute";
    public const string TextureAttributeName          = Namespace + ".TextureAttribute";
    public const string TextureArrayAttributeName     = Namespace + ".TextureArrayAttribute";
    public const string SamplerAttributeName          = Namespace + ".SamplerAttribute";
    public const string UniformBufferAttributeName    = Namespace + ".UniformBufferAttribute";
    public const string StructuredBufferAttributeName = Namespace + ".StructuredBufferAttribute";
    public const string ResourceSetAttributeName      = Namespace + ".ResourceSetAttribute";
    public const string UniformAttributeName          = Namespace + ".UniformAttribute";
    public const string StructuredAttributeName       = Namespace + ".StructuredAttribute";
    public const string VertexAttributeName           = Namespace + ".VertexAttribute";
    public const string VertexShaderAttributeName     = Namespace + ".VertexShaderAttribute";

    public static readonly string[] AttributeNames = [
        ColorAttachmentAttributeName,
        DepthAttachmentAttributeName,
        FragmentShaderAttributeName,
        InputAttributeName,
        NameAttributeName,
        OutputAttributeName,
        TextureAttributeName,
        TextureArrayAttributeName,
        SamplerAttributeName,
        UniformBufferAttributeName,
        StructuredBufferAttributeName,
        ResourceSetAttributeName,
        UniformAttributeName,
        StructuredAttributeName,
        VertexAttributeName,
        VertexShaderAttributeName
    ];

    // Attributes
    public INamedTypeSymbol ColorAttachment  { get; } = VeldridGenUtil.Resolve(compilation, ColorAttachmentAttributeName);
    public INamedTypeSymbol DepthAttachment  { get; } = VeldridGenUtil.Resolve(compilation, DepthAttachmentAttributeName);
    public INamedTypeSymbol FragmentShader   { get; } = VeldridGenUtil.Resolve(compilation, FragmentShaderAttributeName);
    public INamedTypeSymbol Input            { get; } = VeldridGenUtil.Resolve(compilation, InputAttributeName);
    public INamedTypeSymbol Name             { get; } = VeldridGenUtil.Resolve(compilation, NameAttributeName);
    public INamedTypeSymbol Output           { get; } = VeldridGenUtil.Resolve(compilation, OutputAttributeName);
    public INamedTypeSymbol Texture          { get; } = VeldridGenUtil.Resolve(compilation, TextureAttributeName);
    public INamedTypeSymbol TextureArray     { get; } = VeldridGenUtil.Resolve(compilation, TextureArrayAttributeName);
    public INamedTypeSymbol Sampler          { get; } = VeldridGenUtil.Resolve(compilation, SamplerAttributeName);
    public INamedTypeSymbol UniformBuffer    { get; } = VeldridGenUtil.Resolve(compilation, UniformBufferAttributeName);
    public INamedTypeSymbol StructuredBuffer { get; } = VeldridGenUtil.Resolve(compilation, StructuredBufferAttributeName);
    public INamedTypeSymbol ResourceSet      { get; } = VeldridGenUtil.Resolve(compilation, ResourceSetAttributeName);
    public INamedTypeSymbol Uniform          { get; } = VeldridGenUtil.Resolve(compilation, UniformAttributeName);
    public INamedTypeSymbol Structured       { get; } = VeldridGenUtil.Resolve(compilation, StructuredAttributeName);
    public INamedTypeSymbol Vertex           { get; } = VeldridGenUtil.Resolve(compilation, VertexAttributeName);
    public INamedTypeSymbol VertexShader     { get; } = VeldridGenUtil.Resolve(compilation, VertexShaderAttributeName);
}
