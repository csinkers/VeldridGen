using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    public static class EnumUtil
    {
        public static List<(IFieldSymbol field, ulong value)> GetEnumValues(INamedTypeSymbol enumType)
        {
            List<(IFieldSymbol field, ulong value)> results = new();
            foreach (var field in enumType.GetMembers().OfType<IFieldSymbol>())
            {
                var value = GetEnumValue(field);
                if (value.HasValue)
                    results.Add((field, value.Value));
            }

            return results;
        }

        public static ulong? GetEnumValue(IFieldSymbol symbol)
        {
            if (symbol is not {HasConstantValue: true, ConstantValue: not null}) 
                return null;

            var underlyingType = symbol.ContainingType.EnumUnderlyingType;
            if (underlyingType == null) 
                return null;

            return underlyingType.SpecialType switch
            {
                SpecialType.System_SByte => (ulong)(sbyte)symbol.ConstantValue,
                SpecialType.System_Int16 => (ulong)(short)symbol.ConstantValue,
                SpecialType.System_Int32 => (ulong)(int)symbol.ConstantValue,
                SpecialType.System_Int64 => (ulong)(long)symbol.ConstantValue,
                SpecialType.System_Byte => (byte)symbol.ConstantValue,
                SpecialType.System_UInt16 => (ushort)symbol.ConstantValue,
                SpecialType.System_UInt32 => (uint)symbol.ConstantValue,
                SpecialType.System_UInt64 => (ulong)symbol.ConstantValue,
                _ => throw new InvalidOperationException($"{underlyingType} is not a valid underlying type for an enum")
            };
        }
    }
}
