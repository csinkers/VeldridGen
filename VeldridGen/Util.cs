using System;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    static class Util
    {
        public static string UnderscoreToTitleCase(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentOutOfRangeException("Expected non-empty identifier");

            name = name.TrimStart('_');
            if (name.Length == 1)
                return name.ToUpperInvariant();

            return char.ToUpperInvariant(name[0]) + name.Substring(1);
        }

        public static T ToEnum<T>(int numeric) where T : unmanaged, Enum
        {
            unsafe
            {
                var asByte = (byte)numeric;
                var asShort = (ushort)numeric;
                var asInt = numeric;
                return
                    sizeof(T) == 1 ? Unsafe.As<byte, T>(ref asByte)
                    : sizeof(T) == 2 ? Unsafe.As<ushort, T>(ref asShort)
                    : sizeof(T) == 4 ? Unsafe.As<int, T>(ref asInt)
                    : throw new InvalidOperationException($"Type {typeof(T)} is of non-enum type, or has an unsupported underlying type");
            }
        }

        public static INamedTypeSymbol Resolve(Compilation compilation, string name) 
            => compilation.GetTypeByMetadataName(name)
            ?? throw new TypeResolutionException(name);

        /*
        public static string FormatFlagsEnum<T>(T value) where T : unmanaged, Enum
        {
            int intValue = Convert.ToInt32(value);
            int flag = 1;
            StringBuilder sb = new();
            while (flag <= (int)maxValue)
            {
                if ((intValue & flag) != 0)
                {
                    sb.Append(typeof(T).Name);
                    sb.Append('.');
                    sb.Append(ToEnum<T>(flag).ToString());
                    sb.Append(" | ");
                }

                flag <<= 1;
            }

            var result = sb.ToString();
            result = result.TrimEnd(' ').TrimEnd('|').TrimEnd(' ');
            if (string.IsNullOrEmpty(result))
                return "0";
            return result;
        }
        */
        public static INamedTypeSymbol GetFieldOrPropertyType(ISymbol member)
        {
            return member switch
            {
                IFieldSymbol field => (INamedTypeSymbol)field.Type,
                IPropertySymbol property => (INamedTypeSymbol)property.Type,
                _ => throw new ArgumentOutOfRangeException(
                    "Member with a ResourceAttribute was neither a field nor a property")
            };
        }

        public static ISymbol VertexElementFormatForType(ISymbol member, Symbols symbols)
        {
            var type = GetFieldOrPropertyType(member);
            if (type.TypeKind == TypeKind.Enum)
                type = type.EnumUnderlyingType;

            if (type.Equals(symbols.Int, SymbolEqualityComparer.Default)) return symbols.VertexElementFormat.Int1;
            if (type.Equals(symbols.UInt, SymbolEqualityComparer.Default)) return symbols.VertexElementFormat.UInt1;
            if (type.Equals(symbols.Float, SymbolEqualityComparer.Default)) return symbols.VertexElementFormat.Float1;
            if (type.Equals(symbols.Vector2, SymbolEqualityComparer.Default)) return symbols.VertexElementFormat.Float2;
            if (type.Equals(symbols.Vector3, SymbolEqualityComparer.Default)) return symbols.VertexElementFormat.Float3;
            if (type.Equals(symbols.Vector4, SymbolEqualityComparer.Default)) return symbols.VertexElementFormat.Float4;
            throw new ArgumentOutOfRangeException($"Could not find an appropriate vertex element format for field of type {type.ToDisplayString()}");
        }

        public static string GetGlslType(INamedTypeSymbol type, Symbols symbols)
        {
            if (type.TypeKind == TypeKind.Enum)
                type = type.EnumUnderlyingType;

            if (type.Equals(symbols.Byte, SymbolEqualityComparer.Default)) return "byte";
            if (type.Equals(symbols.Short, SymbolEqualityComparer.Default)) return "short";
            if (type.Equals(symbols.UShort, SymbolEqualityComparer.Default)) return "ushort";
            if (type.Equals(symbols.Int, SymbolEqualityComparer.Default)) return "int";
            if (type.Equals(symbols.UInt, SymbolEqualityComparer.Default)) return "uint";
            if (type.Equals(symbols.Float, SymbolEqualityComparer.Default)) return "float";
            if (type.Equals(symbols.Vector2, SymbolEqualityComparer.Default)) return "vec2";
            if (type.Equals(symbols.Vector3, SymbolEqualityComparer.Default)) return "vec3";
            if (type.Equals(symbols.Vector4, SymbolEqualityComparer.Default)) return "vec4";
            if (type.Equals(symbols.Matrix4x4, SymbolEqualityComparer.Default)) return "mat4";
            throw new ArgumentOutOfRangeException(nameof(type), $"Type {type.ToDisplayString()} cannot be converted to a GLSL type");
        }
    }
}
