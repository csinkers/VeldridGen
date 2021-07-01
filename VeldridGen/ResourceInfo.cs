using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    class ResourceInfo
    {
        public ResourceInfo(AttributeData attrib, ISymbol member, GenerationContext context)
        {
            // matching "public ResourceAttribute(string name, ShaderStages stages)", second param optional
            Name = (string)attrib.ConstructorArguments[0].Value;
            // TODO: Verify Name only contains [_a-zA-Z0-9] or something.
            Stages = attrib.ConstructorArguments.Length > 1 && attrib.ConstructorArguments[1].Value != null
                ? (byte)attrib.ConstructorArguments[1].Value
                : (byte)17; // Fragment | Vertex

            (ResourceType, BufferType) = GetKind(member, context);
            Kind = ResourceType switch
            {
                ResourceType.UniformBuffer  => context.Symbols.ResourceKind.UniformBuffer.ToDisplayString(),
                ResourceType.Texture2D      => context.Symbols.ResourceKind.TextureReadOnly.ToDisplayString(),
                ResourceType.Texture2DArray => context.Symbols.ResourceKind.TextureReadOnly.ToDisplayString(),
                ResourceType.Sampler        => context.Symbols.ResourceKind.Sampler.ToDisplayString(),
                _ => throw new ArgumentOutOfRangeException(nameof(ResourceType), $"Unhandled ResourceType: {ResourceType}")
            };

        }

        static (ResourceType, INamedTypeSymbol) GetKind(ISymbol member, GenerationContext context)
        {
            var type = Util.GetFieldOrPropertyType(member);
            var interfaces = new List<INamedTypeSymbol> { type };
            interfaces.AddRange(type.AllInterfaces);

            foreach (var iface in interfaces)
            {
                if (iface.IsGenericType && iface.OriginalDefinition.Equals(context.Symbols.BufferHolder, SymbolEqualityComparer.Default))
                    return (ResourceType.UniformBuffer, (INamedTypeSymbol)iface.TypeArguments[0]);

                if (iface.Equals(context.Symbols.TextureHolder, SymbolEqualityComparer.Default))
                    return (ResourceType.Texture2D, null);

                if (iface.Equals(context.Symbols.TextureArrayHolder, SymbolEqualityComparer.Default))
                    return (ResourceType.Texture2DArray, null);

                if (iface.Equals(context.Symbols.SamplerHolder, SymbolEqualityComparer.Default))
                    return (ResourceType.Sampler, null);
            }

            throw new ArgumentOutOfRangeException(nameof(member), $"Unable to determine a resource kind for field {member.Name} of type {type}");
            // TODO StructuredBufferReadOnly
            // TODO StructuredBufferReadWrite
            // TODO TextureReadWrite
        }

        public string Name { get; }
        public byte Stages { get; }
        public string Kind { get; }
        public ResourceType ResourceType { get; }
        public INamedTypeSymbol BufferType { get; }
    }
}
