using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace VeldridGen;

public class ShaderInfo
{
    public string Filename { get; }
    public ShaderType ShaderType { get; }
    public List<(int slot, INamedTypeSymbol type, int instanceStep)> Inputs { get; } = new(); // slot, type, instance step
    public List<(int slot, INamedTypeSymbol type)> Outputs { get; } = new(); // slot, type
    public List<(int slot, INamedTypeSymbol type)> ResourceSets { get; } = new(); // slot, type

    public ShaderInfo(ShaderType shaderType, INamedTypeSymbol symbol, GenerationContext context)
    {
        ShaderType = shaderType;

        foreach (var attrib in symbol.GetAttributes())
        {
            if (attrib.AttributeClass == null)
                continue;

            if (attrib.AttributeClass.Equals(context.Symbols.Attributes.Name, SymbolEqualityComparer.Default))
            {
                Filename = (string)attrib.ConstructorArguments[0].Value;
            }
            else if (attrib.AttributeClass.Equals(context.Symbols.Attributes.Input, SymbolEqualityComparer.Default))
            {
                var stepArgument =
                    attrib.NamedArguments
                    .Where(x => x.Key == "InstanceStep")
                    .Select(x => (TypedConstant?)x.Value)
                    .SingleOrDefault();

                Inputs.Add((
                    (int)attrib.ConstructorArguments[0].Value,
                    (INamedTypeSymbol)attrib.ConstructorArguments[1].Value,
                    (int?)stepArgument?.Value ?? 0));
            }
            else if (attrib.AttributeClass.Equals(context.Symbols.Attributes.Output, SymbolEqualityComparer.Default))
            {
                Outputs.Add((
                    (int)attrib.ConstructorArguments[0].Value,
                    (INamedTypeSymbol)attrib.ConstructorArguments[1].Value));
            }
            else if (attrib.AttributeClass.Equals(context.Symbols.Attributes.ResourceSet, SymbolEqualityComparer.Default))
            {
                ResourceSets.Add((
                    (int)attrib.ConstructorArguments[0].Value,
                    (INamedTypeSymbol)attrib.ConstructorArguments[1].Value));
            }
        }

        // TODO: Ensure no slot numbers are duplicated
        // TODO: Ensure slots are contiguous and start from 0, or not necessary?

        if (Filename == null)
            throw new InvalidOperationException("No name supplied for shader " + symbol.Name);

        if (Filename.Contains('"') || Filename.Contains('\\'))
            throw new InvalidOperationException($"Filename of shader {symbol.Name} ({Filename}) contains invalid character (\\ or \")");
    }

    public ulong? GetStageFlags(GenerationContext context)
        => EnumUtil.GetEnumValue((IFieldSymbol)(
            ShaderType switch
            {
                ShaderType.Vertex => context.Symbols.Veldrid.ShaderStages.Vertex,
                ShaderType.Fragment => context.Symbols.Veldrid.ShaderStages.Fragment,
                ShaderType.Compute => context.Symbols.Veldrid.ShaderStages.Compute,
                _ => throw new ArgumentOutOfRangeException(nameof(ShaderInfo), $"\"{ShaderType}\" shaders are currently unsupported.")
            }));

}