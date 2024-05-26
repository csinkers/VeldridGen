using Microsoft.CodeAnalysis;

namespace VeldridGen.Symbols;

public class ShaderStageSymbols
{
    public ShaderStageSymbols(Compilation compilation)
    {
        Type = VeldridGenUtil.Resolve(compilation, "Veldrid.ShaderStages");
        foreach (var member in Type.GetMembers())
        {
            switch (member.Name)
            {
                case "Compute": Compute = member; break;
                case "Vertex": Vertex = member; break;
                case "Fragment": Fragment = member; break;
            }
        }

        if (Compute == null) throw new TypeResolutionException("Veldrid.ShaderStages.Compute");
        if (Vertex == null) throw new TypeResolutionException("Veldrid.ShaderStages.Vertex");
        if (Fragment == null) throw new TypeResolutionException("Veldrid.ShaderStages.Fragment");
    }

    public INamedTypeSymbol Type { get; }
    public ISymbol Compute { get; }
    public ISymbol Fragment { get; }
    public ISymbol Vertex { get; }
}