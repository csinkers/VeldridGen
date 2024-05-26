using System;
using Microsoft.CodeAnalysis;

namespace VeldridGen.Symbols;

public class ResourceKindSymbols
{
    public ResourceKindSymbols(Compilation compilation)
    {
        Type = VeldridGenUtil.Resolve(compilation, "Veldrid.ResourceKind");
        foreach (var member in Type.GetMembers())
        {
            if (member.Name == "UniformBuffer") UniformBuffer = member;
            else if (member.Name == "StructuredBufferReadOnly") StructuredBufferReadOnly = member;
            else if (member.Name == "StructuredBufferReadWrite") StructuredBufferReadWrite = member;
            else if (member.Name == "TextureReadOnly") TextureReadOnly = member;
            else if (member.Name == "TextureReadWrite") TextureReadWrite = member;
            else if (member.Name == "Sampler") Sampler = member;
        }

        if (UniformBuffer == null) throw new TypeResolutionException("Veldrid.ResourceKind.UniformBuffer");
        if (StructuredBufferReadOnly == null) throw new TypeResolutionException("Veldrid.ResourceKind.StructuredBufferReadOnly");
        if (StructuredBufferReadWrite == null) throw new TypeResolutionException("Veldrid.ResourceKind.StructuredBufferReadWrite");
        if (TextureReadOnly == null) throw new TypeResolutionException("Veldrid.ResourceKind.TextureReadOnly");
        if (TextureReadWrite == null) throw new TypeResolutionException("Veldrid.ResourceKind.TextureReadWrite");
        if (Sampler == null) throw new TypeResolutionException("Veldrid.ResourceKind.Sampler");
    }


    public INamedTypeSymbol Type { get; }
    public ISymbol Sampler { get; }
    public ISymbol TextureReadOnly { get; }
    public ISymbol TextureReadWrite { get; }
    public ISymbol UniformBuffer { get; }
    public ISymbol StructuredBufferReadOnly { get; }
    public ISymbol StructuredBufferReadWrite { get; }

    public string KnownKindString(KnownResourceKind kind) =>
        kind switch
        {
            KnownResourceKind.UniformBuffer => UniformBuffer.ToDisplayString(),
            KnownResourceKind.StructuredBufferReadOnly => StructuredBufferReadOnly.ToDisplayString(),
            KnownResourceKind.StructuredBufferReadWrite => StructuredBufferReadWrite.ToDisplayString(),
            KnownResourceKind.TextureReadOnly => TextureReadOnly.ToDisplayString(),
            KnownResourceKind.TextureReadWrite => TextureReadWrite.ToDisplayString(),
            KnownResourceKind.Sampler => Sampler.ToDisplayString(),
            KnownResourceKind.Unknown => null,
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, $"Unexpected Kind {kind} encountered in ResourceKindSymbols.KnownKindString")
        };
}