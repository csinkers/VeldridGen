using System;
using Microsoft.CodeAnalysis;

namespace VeldridGen;

public class VeldridMemberInfo
{
    public VeldridMemberInfo(ISymbol symbol, GenerationContext context)
    {
        T Try<T>(string locus, Func<T> func) where T : class
        {
            try { return func(); }
            catch (Exception e)
            {
                context.Error($"{symbol.ContainingType.ToDisplayString()}.{symbol.ToDisplayString()} could not be initialised as a {locus}: {e.Message}");
                return null;
            }
        }

        Symbol = symbol;
        Type = VeldridGenUtil.GetFieldOrPropertyType(symbol);
        foreach (var attrib in symbol.GetAttributes())
        {
            if (attrib.AttributeClass == null)
                continue;

            if (attrib.AttributeClass.Equals(context.Symbols.Attributes.ColorAttachment, SymbolEqualityComparer.Default))
                ColorAttachment = Try("color attachment", () => new ColorAttachmentInfo(attrib));
            else if (attrib.AttributeClass.Equals(context.Symbols.Attributes.DepthAttachment, SymbolEqualityComparer.Default))
                DepthAttachment = Try("depth attachment", () => new DepthAttachmentInfo(attrib));
            else if (attrib.AttributeClass.Equals(context.Symbols.Attributes.Vertex, SymbolEqualityComparer.Default))
                Vertex = Try("vertex info", () => new VertexInfo(attrib, symbol, context));
            else if (attrib.AttributeClass.Equals(context.Symbols.Attributes.Uniform, SymbolEqualityComparer.Default))
                UniformMember = Try("uniform", () => new UniformMemberInfo(attrib));
            else if (attrib.AttributeClass.Equals(context.Symbols.Attributes.Structured, SymbolEqualityComparer.Default))
                StructuredMember = Try("structured", () => new StructuredMemberInfo(attrib));
            else
            {
                var resourceType = ResourceType.Unknown;
                if (attrib.AttributeClass.Equals(context.Symbols.Attributes.Texture, SymbolEqualityComparer.Default))
                    resourceType = ResourceType.Texture2D;
                else if (attrib.AttributeClass.Equals(context.Symbols.Attributes.TextureArray, SymbolEqualityComparer.Default))
                    resourceType = ResourceType.Texture2DArray;
                else if (attrib.AttributeClass.Equals(context.Symbols.Attributes.Sampler, SymbolEqualityComparer.Default))
                    resourceType = ResourceType.Sampler;
                else if (attrib.AttributeClass.Equals(context.Symbols.Attributes.UniformBuffer, SymbolEqualityComparer.Default))
                    resourceType = ResourceType.UniformBuffer;
                else if (attrib.AttributeClass.Equals(context.Symbols.Attributes.StructuredBuffer, SymbolEqualityComparer.Default))
                    resourceType = ResourceType.StructuredBuffer;

                if (resourceType != ResourceType.Unknown)
                {
                    Resource = Try("resource", () => new ResourceInfo(
                        resourceType,
                        AttribName(attrib),
                        AttribStages(attrib),
                        AttribIsReadOnly(attrib),
                        symbol,
                        context));
                }
            }
        }

        IsRelevant =
            ColorAttachment != null ||
            DepthAttachment != null ||
                     Vertex != null ||
                   Resource != null ||
              UniformMember != null ||
           StructuredMember != null;
    }

    static string AttribName(AttributeData attrib)
    {
        if (attrib.ConstructorArguments.Length == 0)
            throw new ArgumentOutOfRangeException($"Expected Attribute {attrib} to have a name string as the first constructor parameter");
        return (string)attrib.ConstructorArguments[0].Value;
    }

    static bool AttribIsReadOnly(AttributeData attrib)
    {
        foreach (var arg in attrib.ConstructorArguments)
            if (arg.Value is bool isReadOnly)
                return isReadOnly;

        return true;
    }

    static byte AttribStages(AttributeData attrib)
    {
        foreach (var arg in attrib.ConstructorArguments)
            if (arg.Value is byte stages)
                return stages;

        return 17; // Fragment | Vertex
    }

    public ISymbol Symbol { get; }
    public INamedTypeSymbol Type { get; }
    public ColorAttachmentInfo ColorAttachment { get; }
    public DepthAttachmentInfo DepthAttachment { get; }
    public ResourceInfo Resource { get; }
    public UniformMemberInfo UniformMember { get; }
    public StructuredMemberInfo StructuredMember { get; }
    public VertexInfo Vertex { get; }
    public bool IsRelevant { get; }
}
