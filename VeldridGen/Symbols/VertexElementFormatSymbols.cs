using Microsoft.CodeAnalysis;

namespace VeldridGen.Symbols;

public class VertexElementFormatSymbols
{
    public VertexElementFormatSymbols(Compilation compilation)
    {
        Type = VeldridGenUtil.Resolve(compilation, "Veldrid.VertexElementFormat");
        foreach (var member in Type.GetMembers())
        {
            if (member.Name == "Int1") Int1 = member;
            else if (member.Name == "UInt1") UInt1 = member;
            else if (member.Name == "Float1") Float1 = member;
            else if (member.Name == "Float2") Float2 = member;
            else if (member.Name == "Float3") Float3 = member;
            else if (member.Name == "Float4") Float4 = member;
        }

        if (Int1 == null) throw new TypeResolutionException("Veldrid.VertexElementFormat.Int1");
        if (UInt1 == null) throw new TypeResolutionException("Veldrid.VertexElementFormat.UInt1");
        if (Float1 == null) throw new TypeResolutionException("Veldrid.VertexElementFormat.Float1");
        if (Float2 == null) throw new TypeResolutionException("Veldrid.VertexElementFormat.Float2");
        if (Float3 == null) throw new TypeResolutionException("Veldrid.VertexElementFormat.Float3");
        if (Float4 == null) throw new TypeResolutionException("Veldrid.VertexElementFormat.Float4");
    }

    public INamedTypeSymbol Type { get; }
    public ISymbol Int1 { get; }
    public ISymbol UInt1 { get; }
    public ISymbol Float1 { get; }
    public ISymbol Float2 { get; }
    public ISymbol Float3 { get; }
    public ISymbol Float4 { get; }
}
