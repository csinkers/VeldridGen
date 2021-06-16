using System;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    class ResourceInfo
    {
        public ResourceInfo(AttributeData attrib, ISymbol member, Symbols symbols)
        {
            // matching "public ResourceAttribute(string name, ShaderStages stages)", second param optional
            Name = (string)attrib.ConstructorArguments[0].Value;
            Stages = attrib.ConstructorArguments.Length > 1 && attrib.ConstructorArguments[1].Value != null
                ? (byte)attrib.ConstructorArguments[1].Value
                : (byte)17; // Fragment | Vertex
            Kind = GetKind(member, symbols);
        }

        static string GetKind(ISymbol member, Symbols symbols)
        {
            var type = Util.GetFieldOrPropertyType(member);
            if (type.AllInterfaces.Any(x => x.Equals(symbols.BufferHolder, SymbolEqualityComparer.Default)))
                return symbols.ResourceKind.UniformBuffer.ToDisplayString();

            if (type.AllInterfaces.Any(x => x.Equals(symbols.TextureHolder, SymbolEqualityComparer.Default)))
                return symbols.ResourceKind.TextureReadOnly.ToDisplayString();

            if (type.AllInterfaces.Any(x => x.Equals(symbols.SamplerHolder, SymbolEqualityComparer.Default)))
                return symbols.ResourceKind.Sampler.ToDisplayString();

            throw new ArgumentOutOfRangeException($"Unable to determine a resource kind for field {member.Name} of type {type}");
            // TODO StructuredBufferReadOnly
            // TODO StructuredBufferReadWrite
            // TODO TextureReadWrite
        }

        public string Name { get; }
        public byte Stages { get; }
        public string Kind { get; }
    }
}
