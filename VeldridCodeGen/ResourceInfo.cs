using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Veldrid;

namespace VeldridCodeGen
{
    class ResourceInfo
    {
        public ResourceInfo(AttributeData attrib, ISymbol member, Symbols symbols)
        {
            // matching "public ResourceAttribute(string name, ShaderStages stages)", second param optional
            Name = (string)attrib.ConstructorArguments[0].Value;
            Stages = attrib.ConstructorArguments.Length > 1 && attrib.ConstructorArguments[1].Value != null
                ? (ShaderStages)attrib.ConstructorArguments[1].Value
                : ShaderStages.Fragment | ShaderStages.Vertex;
            Kind = GetKind(member, symbols);
        }

        ResourceKind GetKind(ISymbol member, Symbols symbols)
        {
            ITypeSymbol type = member switch
            {
                IFieldSymbol field => field.Type,
                IPropertySymbol property => property.Type,
                _ => throw new ArgumentOutOfRangeException(
                    "Member with a ResourceAttribute was neither a field nor a property")
            };

            if (type.AllInterfaces.Any(x => x.Equals(symbols.BufferHolder, SymbolEqualityComparer.Default)))
                return ResourceKind.UniformBuffer;

            if (type.AllInterfaces.Any(x => x.Equals(symbols.TextureHolder, SymbolEqualityComparer.Default)))
                return ResourceKind.TextureReadOnly;

            if (type.AllInterfaces.Any(x => x.Equals(symbols.SamplerHolder, SymbolEqualityComparer.Default)))
                return ResourceKind.Sampler;

            throw new ArgumentOutOfRangeException($"Unable to determine a resource kind for field {member.Name} of type {type}");
            // TODO StructuredBufferReadOnly
            // TODO StructuredBufferReadWrite
            // TODO TextureReadWrite
        }

        public string Name { get; }
        public ShaderStages Stages { get; }
        public ResourceKind Kind { get; }
    }
}