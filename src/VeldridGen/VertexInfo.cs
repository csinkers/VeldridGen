using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace VeldridGen;

public class VertexInfo
{
    public VertexInfo(AttributeData attrib, ISymbol symbol, GenerationContext context)
    {
        // matching "public InputParamAttribute(string name, VertexElementFormat format)" (second param optional)
        Name = (string)attrib.ConstructorArguments[0].Value;
        Format = attrib.ConstructorArguments.Length > 1 && attrib.ConstructorArguments[1].Value != null
            ? attrib.ConstructorArguments[1].ToCSharpString()
            : VeldridGenUtil.VertexElementFormatForType(symbol, context.Symbols).ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

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
}
