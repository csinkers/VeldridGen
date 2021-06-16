using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace VeldridGen
{
    class VertexInfo
    {
        public VertexInfo(AttributeData attrib, ISymbol symbol, Symbols symbols)
        {
            // matching "public InputParamAttribute(string name, VertexElementFormat format)" (second param optional)
            Name = (string)attrib.ConstructorArguments[0].Value;
            Format = attrib.ConstructorArguments.Length > 1 && attrib.ConstructorArguments[1].Value != null
                ? attrib.ConstructorArguments[1].ToCSharpString()
                : FormatForType(symbol, symbols).ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
            Flat = 
                (bool?)attrib.NamedArguments
                .Where(x => x.Key == "Flat")
                .Select(x => (TypedConstant?)x.Value)
                .SingleOrDefault()?.Value ?? false;

            EnumPrefix = 
                (string)attrib.NamedArguments
                .Where(x => x.Key == "EnumPrefix")
                .Select(x => (TypedConstant?)x.Value)
                .SingleOrDefault()?.Value;
        }

        public string Name { get; }
        public bool Flat { get; }
        public string Format { get; }
        public string EnumPrefix { get; }

        static ISymbol FormatForType(ISymbol member, Symbols symbols)
        {
            var type = Util.GetFieldOrPropertyType(member);
            if (type.TypeKind == TypeKind.Enum)
            {
                var named = (INamedTypeSymbol)type;
                type = named.EnumUnderlyingType;
            }

            if (type.Equals(symbols.Int, SymbolEqualityComparer.Default)) return symbols.VertexElementFormat.Int1;
            if (type.Equals(symbols.UInt, SymbolEqualityComparer.Default)) return symbols.VertexElementFormat.UInt1;
            if (type.Equals(symbols.Float, SymbolEqualityComparer.Default)) return symbols.VertexElementFormat.Float1;
            if (type.Equals(symbols.Vector2, SymbolEqualityComparer.Default)) return symbols.VertexElementFormat.Float2;
            if (type.Equals(symbols.Vector3, SymbolEqualityComparer.Default)) return symbols.VertexElementFormat.Float3;
            if (type.Equals(symbols.Vector4, SymbolEqualityComparer.Default)) return symbols.VertexElementFormat.Float4;
            throw new ArgumentOutOfRangeException($"Could not find an appropriate vertex element format for field of type {type.ToDisplayString()}");
        }
    }
}
