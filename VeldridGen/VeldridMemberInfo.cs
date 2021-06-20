using System;
using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    class VeldridMemberInfo
    {
        public VeldridMemberInfo(ISymbol symbol, GenerationContext context)
        {
            T Try<T>(string locus, Func<T> func) where T : class
            {
                try { return func(); }
                catch (Exception e)
                {
                    context.Report($"{symbol.ContainingType.ToDisplayString()}.{symbol.ToDisplayString()} could not be initialised as a {locus}: {e.Message}");
                    return null;
                }
            }

            Symbol = symbol;
            Type = Util.GetFieldOrPropertyType(symbol);
            foreach (var attrib in symbol.GetAttributes())
            {
                if (attrib.AttributeClass == null)
                    continue;

                if (attrib.AttributeClass.Equals(context.Symbols.ColorAttachmentAttrib, SymbolEqualityComparer.Default))
                    ColorAttachment = Try("color attachment", () => new ColorAttachmentInfo(attrib));
                else if (attrib.AttributeClass.Equals(context.Symbols.DepthAttachmentAttrib, SymbolEqualityComparer.Default))
                    DepthAttachment = Try("depth attachment", () => new DepthAttachmentInfo(attrib));
                else if (attrib.AttributeClass.Equals(context.Symbols.VertexAttrib, SymbolEqualityComparer.Default))
                    Vertex = Try("vertex info", () => new VertexInfo(attrib, symbol, context));
                else if (attrib.AttributeClass.Equals(context.Symbols.ResourceAttrib, SymbolEqualityComparer.Default))
                    Resource = Try("resource", () => new ResourceInfo(attrib, symbol, context));
                else if (attrib.AttributeClass.Equals(context.Symbols.UniformAttrib, SymbolEqualityComparer.Default))
                    Uniform = Try("uniform", () => new UniformInfo(attrib));
            }

            IsRelevant = 
                ColorAttachment != null ||
                DepthAttachment != null ||
                         Vertex != null ||
                       Resource != null ||
                        Uniform != null;
        }

        public ISymbol Symbol { get; }
        public INamedTypeSymbol Type { get; }
        public ColorAttachmentInfo ColorAttachment { get; }
        public DepthAttachmentInfo DepthAttachment { get; }
        public ResourceInfo Resource { get; }
        public UniformInfo Uniform { get; }
        public VertexInfo Vertex { get; }
        public bool IsRelevant { get; }
    }
}
