using System;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    public static class VeldridGenUtil
    {
        public static string UnderscoreToTitleCase(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentOutOfRangeException(nameof(name), "Expected non-empty identifier");

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
                return sizeof(T) == 1 ? Unsafe.As<byte, T>(ref asByte)
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
                _ => throw new ArgumentOutOfRangeException(nameof(member), "Member with a ResourceAttribute was neither a field nor a property")
            };
        }

        public static ISymbol VertexElementFormatForType(ISymbol member, Symbols symbols)
        {
            var type = GetFieldOrPropertyType(member);
            if (type.TypeKind == TypeKind.Enum)
                type = type.EnumUnderlyingType;

            if (type == null)
                throw new ArgumentNullException(nameof(member), $"Could not find an appropriate vertex element format for member \"{member.ToDisplayString()}\"");
                
            if (type.Equals(symbols.Int, SymbolEqualityComparer.Default)) return symbols.VertexElementFormat.Int1;
            if (type.Equals(symbols.UInt, SymbolEqualityComparer.Default)) return symbols.VertexElementFormat.UInt1;
            if (type.Equals(symbols.Float, SymbolEqualityComparer.Default)) return symbols.VertexElementFormat.Float1;
            if (type.Equals(symbols.Vector2, SymbolEqualityComparer.Default)) return symbols.VertexElementFormat.Float2;
            if (type.Equals(symbols.Vector3, SymbolEqualityComparer.Default)) return symbols.VertexElementFormat.Float3;
            if (type.Equals(symbols.Vector4, SymbolEqualityComparer.Default)) return symbols.VertexElementFormat.Float4;
            throw new ArgumentOutOfRangeException(nameof(member), $"Could not find an appropriate vertex element format for field of type {type.ToDisplayString()}");
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

        public static string GetGlslTypeForColorAttachment(string format)
        {
            return format switch
            {
                "Veldrid.PixelFormat.R8_SNorm" => "uint",
                "Veldrid.PixelFormat.R8_UInt" => "uint",
                "Veldrid.PixelFormat.R8_UNorm" => "uint",
                "Veldrid.PixelFormat.R16_UInt" => "uint",
                "Veldrid.PixelFormat.R16_UNorm" => "uint",
                "Veldrid.PixelFormat.R32_UInt" => "uint",

                "Veldrid.PixelFormat.R8_SInt" => "int",
                "Veldrid.PixelFormat.R16_SNorm" => "int",
                "Veldrid.PixelFormat.R16_SInt" => "int",
                "Veldrid.PixelFormat.R32_SInt" => "int",

                "Veldrid.PixelFormat.R16_Float" => "float",
                "Veldrid.PixelFormat.R32_Float" => "float",

                "Veldrid.PixelFormat.R11_G11_B10_Float" => "vec3",

                "Veldrid.PixelFormat.R8_G8_B8_A8_UInt" => "vec4",
                "Veldrid.PixelFormat.R8_G8_B8_A8_SInt" => "vec4",
                "Veldrid.PixelFormat.R8_G8_B8_A8_SNorm" => "vec4",
                "Veldrid.PixelFormat.R8_G8_B8_A8_UNorm" => "vec4",
                "Veldrid.PixelFormat.B8_G8_R8_A8_UNorm" => "vec4",
                "Veldrid.PixelFormat.R8_G8_B8_A8_UNorm_SRgb" => "vec4",
                "Veldrid.PixelFormat.B8_G8_R8_A8_UNorm_SRgb" => "vec4",
                "Veldrid.PixelFormat.R10_G10_B10_A2_UNorm" => "vec4",
                "Veldrid.PixelFormat.R10_G10_B10_A2_UInt" => "vec4",
                "Veldrid.PixelFormat.R16_G16_B16_A16_UNorm" => "vec4",
                "Veldrid.PixelFormat.R16_G16_B16_A16_SNorm" => "vec4",
                "Veldrid.PixelFormat.R16_G16_B16_A16_UInt" => "vec4",
                "Veldrid.PixelFormat.R16_G16_B16_A16_SInt" => "vec4",
                "Veldrid.PixelFormat.R16_G16_B16_A16_Float" => "vec4",
                "Veldrid.PixelFormat.R32_G32_B32_A32_Float" => "vec4",
                "Veldrid.PixelFormat.R32_G32_B32_A32_SInt" => "vec4",
                "Veldrid.PixelFormat.R32_G32_B32_A32_UInt" => "vec4",
                _ => throw new FormatException($"Unhandled pixel format \"{format}\"")
            };
        }
    }
}
