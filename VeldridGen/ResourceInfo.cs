using System;
using Microsoft.CodeAnalysis;

namespace VeldridGen;

public class ResourceInfo
{
    public ResourceInfo(ResourceType resourceType, string name, byte stages, bool isReadOnly, ISymbol member, GenerationContext context)
    {
        // matching "public ResourceAttribute(string name, ShaderStages stages)", second param optional
        ResourceType = resourceType;
        Name = name; // TODO: Verify Name only contains [_a-zA-Z0-9] or something.
        Stages = stages;
        Kind = (ResourceType, isReadOnly) switch
        {
            (ResourceType.UniformBuffer, _) => KnownResourceKind.UniformBuffer,
            (ResourceType.StructuredBuffer, true) => KnownResourceKind.StructuredBufferReadOnly,
            (ResourceType.StructuredBuffer, false) => KnownResourceKind.StructuredBufferReadWrite,
            (ResourceType.Texture2D, true) => KnownResourceKind.TextureReadOnly,
            (ResourceType.Texture2D, false) => KnownResourceKind.TextureReadWrite,
            (ResourceType.Texture2DArray, true) => KnownResourceKind.TextureReadOnly,
            (ResourceType.Texture2DArray, false) => KnownResourceKind.TextureReadWrite,
            (ResourceType.Sampler, _) => KnownResourceKind.Sampler,
            _ => throw new ArgumentOutOfRangeException(nameof(ResourceType), $"Unhandled ResourceType: {ResourceType}")
        };

        BufferType = ResourceType switch
        {
            ResourceType.Sampler => VerifyInterfaceAndExtractBufferType(member, context.Symbols.Interfaces.SamplerHolder, ResourceType),
            ResourceType.UniformBuffer => VerifyInterfaceAndExtractBufferType(member, context.Symbols.Interfaces.BufferHolder, ResourceType),
            ResourceType.StructuredBuffer => VerifyInterfaceAndExtractBufferType(member, context.Symbols.Interfaces.BufferHolder, ResourceType),
            ResourceType.Texture2D => VerifyInterfaceAndExtractBufferType(member, context.Symbols.Interfaces.TextureHolder, ResourceType),
            ResourceType.Texture2DArray => VerifyInterfaceAndExtractBufferType(member, context.Symbols.Interfaces.TextureArrayHolder, ResourceType),
            _ => null
        };
    }

    static INamedTypeSymbol VerifyInterfaceAndExtractBufferType(ISymbol member, INamedTypeSymbol expectedInterface, ResourceType resourceType)
    {
        var type = VeldridGenUtil.GetFieldOrPropertyType(member);
        if (type.IsGenericType && type.OriginalDefinition.Equals(expectedInterface, SymbolEqualityComparer.Default))
            return (INamedTypeSymbol)type.TypeArguments[0];

        if (type.Equals(expectedInterface, SymbolEqualityComparer.Default))
            return null;

        foreach (var iface in type.AllInterfaces)
        {
            if (iface.IsGenericType && iface.OriginalDefinition.Equals(expectedInterface, SymbolEqualityComparer.Default))
                return (INamedTypeSymbol)iface.TypeArguments[0];

            if (iface.Equals(expectedInterface, SymbolEqualityComparer.Default))
                return null;
        }

        throw new ArgumentOutOfRangeException($"Expected {member} of type {type} to implement the {expectedInterface} interface, as it is of ResourceType {resourceType}");
    }

    public string Name { get; }
    public byte Stages { get; }
    public KnownResourceKind Kind { get; }
    public ResourceType ResourceType { get; }
    public INamedTypeSymbol BufferType { get; }
}