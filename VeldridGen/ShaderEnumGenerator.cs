using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    static class ShaderEnumGenerator
    {
        public static void EmitEnums(StringBuilder sb, VeldridTypeInfo shaderType, Dictionary<INamedTypeSymbol, VeldridTypeInfo> types)
        {
            var enumTypes = FindEnumTypes(shaderType, types);
            foreach (var kvp in enumTypes)
            {
                sb.Append("// ");
                sb.AppendLine(kvp.Key.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
                var underlying = kvp.Key.EnumUnderlyingType.SpecialType;
                var affix = EnumAffix(underlying);

                foreach (var value in GetEnumValues(kvp.Key).OrderBy(x => x.value))
                {
                    sb.Append("#define ");
                    sb.Append(TweakEnumMemberName(value.field.Name, kvp.Value));
                    sb.Append(" 0x");
                    sb.AppendFormat("{0:X}", value.value);
                    sb.AppendLine(affix);
                }

                sb.AppendLine();
            }
        }

        static List<(IFieldSymbol field, ulong value)> GetEnumValues(INamedTypeSymbol enumType)
        {
            List<(IFieldSymbol field, ulong value)> results = new ();
            var underlying = enumType.EnumUnderlyingType.SpecialType;
            foreach (var field in enumType.GetMembers().OfType<IFieldSymbol>())
            {
                if (field is { HasConstantValue: true, ConstantValue: not null })
                {
                    var value = ConvertEnumValue(field.ConstantValue, underlying);
                    results.Add((field, value));
                }
            }

            return results;
        }

        static ulong ConvertEnumValue(object value, SpecialType underlyingType) =>
            underlyingType switch
            {
                SpecialType.System_SByte => (ulong)(sbyte)value,
                SpecialType.System_Int16 => (ulong)(short)value,
                SpecialType.System_Int32 => (ulong)(int)value,
                SpecialType.System_Int64 => (ulong)(long)value,
                SpecialType.System_Byte => (byte)value,
                SpecialType.System_UInt16 => (ushort)value,
                SpecialType.System_UInt32 => (uint)value,
                SpecialType.System_UInt64 => (ulong)value,
                _ => throw new InvalidOperationException($"{underlyingType} is not a valid underlying type for an enum")
            };

        static string EnumAffix(SpecialType underlyingType) =>
            underlyingType switch
            {
                SpecialType.System_Int64 => "L",
                SpecialType.System_UInt32 => "U",
                SpecialType.System_UInt64 => "UL",
                _ => ""
            };

        static Dictionary<INamedTypeSymbol, string> FindEnumTypes(VeldridTypeInfo shaderType, Dictionary<INamedTypeSymbol, VeldridTypeInfo> types)
        {
            var allTypeSymbols = new List<INamedTypeSymbol>();
            allTypeSymbols.AddRange(shaderType.Shader.Inputs.Select(x => x.Item2));
            allTypeSymbols.AddRange(shaderType.Shader.Outputs.Select(x => x.Item2));

            foreach (var resourceSetType in shaderType.Shader.ResourceSets.Select(x => x.Item2))
            {
                if (!types.TryGetValue(resourceSetType, out var resourceSetTypeInfo))
                    continue;

                allTypeSymbols.AddRange(resourceSetTypeInfo.Members
                    .Where(x => x.Resource?.BufferType != null)
                    .Select(resourceType => resourceType.Resource.BufferType));
            }

            var enumTypes = new Dictionary<INamedTypeSymbol, string>(SymbolEqualityComparer.Default);
            foreach (var typeSymbol in allTypeSymbols)
            {
                if (!types.TryGetValue(typeSymbol, out var typeInfo))
                    continue;

                foreach (var member in typeInfo.Members)
                {
                    var prefix = member.Vertex?.EnumPrefix ?? member.Uniform?.EnumPrefix;
                    if (prefix == null)
                        continue;

                    INamedTypeSymbol memberType;
                    switch (member.Symbol)
                    {
                        case IPropertySymbol prop: memberType = (INamedTypeSymbol)prop.Type; break;
                        case IFieldSymbol field:   memberType = (INamedTypeSymbol)field.Type; break;
                        default: continue;
                    }

                    if (memberType.TypeKind != TypeKind.Enum) 
                        continue;

                    if (enumTypes.TryGetValue(memberType, out var existingPrefix))
                    {
                        if (existingPrefix != prefix)
                        {
                            throw new InvalidOperationException(
                                $"Member {member.Symbol.Name} of {typeSymbol.Name} was declared with enum " +
                                $"prefix \"{prefix}\", but another attribute has already declared it as \"{existingPrefix}\"");
                        }
                    }
                    else enumTypes[memberType] = prefix;
                }
            }

            return enumTypes;
        }

        static string TweakEnumMemberName(string name, string prefix)
        {
            var sb = new StringBuilder();
            sb.Append(prefix);
            sb.Append('_');
            bool wasCapital = true;
            foreach (var c in name)
            {
                if (char.IsUpper(c))
                {
                    if (!wasCapital)
                        sb.Append('_');
                    wasCapital = true;
                }
                else wasCapital = false;
                sb.Append(char.ToUpperInvariant(c));
            }
            return sb.ToString();
        }
    }
}