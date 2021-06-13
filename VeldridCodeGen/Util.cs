using System;
using System.Runtime.CompilerServices;
using Microsoft.CodeAnalysis;

namespace VeldridCodeGen
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
        public static ITypeSymbol GetFieldOrPropertyType(ISymbol member)
        {
            return member switch
            {
                IFieldSymbol field => field.Type,
                IPropertySymbol property => property.Type,
                _ => throw new ArgumentOutOfRangeException(
                    "Member with a ResourceAttribute was neither a field nor a property")
            };
        }
    }
}