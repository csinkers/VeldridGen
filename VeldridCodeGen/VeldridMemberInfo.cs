using Microsoft.CodeAnalysis;

namespace VeldridCodeGen
{
    class VeldridMemberInfo
    {
        public VeldridMemberInfo(ISymbol symbol, Symbols symbols)
        {
            Symbol = symbol;
            foreach (var attrib in symbol.GetAttributes())
            {
                if (attrib.AttributeClass == null)
                    continue;

                if (attrib.AttributeClass.Equals(symbols.ColorAttachmentAttrib, SymbolEqualityComparer.Default))
                {
                    Flags |= MemberFlags.IsColorAttachment;
                    ColorAttachment = new ColorAttachmentInfo(attrib);
                }
                else if (attrib.AttributeClass.Equals(symbols.DepthAttachmentAttrib, SymbolEqualityComparer.Default))
                {
                    Flags |= MemberFlags.IsDepthAttachment;
                    DepthAttachment = new DepthAttachmentInfo(attrib);
                }
                else if (attrib.AttributeClass.Equals(symbols.VertexAttrib, SymbolEqualityComparer.Default))
                {
                    Flags |= MemberFlags.IsVertexComponent;
                    Vertex = new VertexInfo(attrib, symbol, symbols);
                }
                else if (attrib.AttributeClass.Equals(symbols.ResourceAttrib, SymbolEqualityComparer.Default))
                {
                    Flags |= MemberFlags.IsResource;
                    Resource = new ResourceInfo(attrib, symbol, symbols);
                }
            }
        }

        public ISymbol Symbol { get; }
        public MemberFlags Flags { get; }
        public ResourceInfo Resource { get; }
        public VertexInfo Vertex { get; }
        public ColorAttachmentInfo ColorAttachment { get; }
        public DepthAttachmentInfo DepthAttachment { get; }
    }
}