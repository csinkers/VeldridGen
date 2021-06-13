using System;
using Microsoft.CodeAnalysis;
using Veldrid;

namespace VeldridCodeGen
{
    class InputInfo
    {
        public InputInfo(AttributeData attrib, ISymbol symbol, Symbols symbols)
        {
            // matching "public InputParamAttribute(string name, VertexElementFormat format)" (second param optional)
            Name = (string)attrib.ConstructorArguments[0].Value;
            Format = attrib.ConstructorArguments.Length > 1 && attrib.ConstructorArguments[1].Value != null
                ? (VertexElementFormat) attrib.ConstructorArguments[1].Value
                : FormatForType(symbol, symbols);
        }

        public string Name { get; }
        public VertexElementFormat Format { get; }

        VertexElementFormat FormatForType(ISymbol type, Symbols symbols)
        {
            if (type.Equals(symbols.Int, SymbolEqualityComparer.Default)) return VertexElementFormat.Int1;
            if (type.Equals(symbols.UInt, SymbolEqualityComparer.Default)) return VertexElementFormat.UInt1;
            if (type.Equals(symbols.Float, SymbolEqualityComparer.Default)) return VertexElementFormat.Float1;
            if (type.Equals(symbols.Vector2, SymbolEqualityComparer.Default)) return VertexElementFormat.Float2;
            if (type.Equals(symbols.Vector3, SymbolEqualityComparer.Default)) return VertexElementFormat.Float3;
            if (type.Equals(symbols.Vector4, SymbolEqualityComparer.Default)) return VertexElementFormat.Float4;
            throw new ArgumentOutOfRangeException($"Could not find an appropriate vertex element format for field of type {type.ToDisplayString()}");
        }
    }
}