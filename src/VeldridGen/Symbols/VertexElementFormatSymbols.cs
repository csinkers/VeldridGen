﻿using Microsoft.CodeAnalysis;

namespace VeldridGen.Symbols;

public class VertexElementFormatSymbols
{
    public VertexElementFormatSymbols(Compilation compilation)
    {
        Type = VeldridGenUtil.Resolve(compilation, "Veldrid.VertexElementFormat");
        foreach (var member in Type.GetMembers())
        {
            switch (member.Name)
            {
                case "Int1":   Int1   = member; break;
                case "UInt1":  UInt1  = member; break;
                case "Float1": Float1 = member; break;
                case "Float2": Float2 = member; break;
                case "Float3": Float3 = member; break;
                case "Float4": Float4 = member; break;
            }
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
