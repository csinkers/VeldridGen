using System;
using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    class ResourceInfo
    {
        public ResourceInfo(AttributeData attrib, ISymbol member, Symbols symbols)
        {
            // matching "public ResourceAttribute(string name, ShaderStages stages)", second param optional
            Name = (string)attrib.ConstructorArguments[0].Value;
            // TODO: Verify Name only contains [_a-zA-Z0-9] or something.
            Stages = attrib.ConstructorArguments.Length > 1 && attrib.ConstructorArguments[1].Value != null
                ? (byte)attrib.ConstructorArguments[1].Value
                : (byte)17; // Fragment | Vertex

            (ResourceType, BufferType) = GetKind(member, symbols);
            Kind = ResourceType switch
            {
                ResourceType.UniformBuffer  => symbols.ResourceKind.UniformBuffer.ToDisplayString(),
                ResourceType.Texture2D      => symbols.ResourceKind.TextureReadOnly.ToDisplayString(),
                ResourceType.Texture2DArray => symbols.ResourceKind.TextureReadOnly.ToDisplayString(),
                ResourceType.Sampler        => symbols.ResourceKind.Sampler.ToDisplayString(),
                _ => throw new ArgumentOutOfRangeException()
            };

        }

        static (ResourceType, INamedTypeSymbol) GetKind(ISymbol member, Symbols symbols)
        {
            var type = Util.GetFieldOrPropertyType(member);
            foreach (var iface in type.AllInterfaces)
            {
                if (iface.IsGenericType && iface.OriginalDefinition.Equals(symbols.BufferHolder, SymbolEqualityComparer.Default))
                    return (ResourceType.UniformBuffer, (INamedTypeSymbol)iface.TypeArguments[0]);

                if (iface.Equals(symbols.TextureHolder, SymbolEqualityComparer.Default))
                    return (ResourceType.Texture2D, null);

                if (iface.Equals(symbols.TextureArrayHolder, SymbolEqualityComparer.Default))
                    return (ResourceType.Texture2DArray, null);

                if (iface.Equals(symbols.SamplerHolder, SymbolEqualityComparer.Default))
                    return (ResourceType.Sampler, null);
            }

            throw new ArgumentOutOfRangeException($"Unable to determine a resource kind for field {member.Name} of type {type}");
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
