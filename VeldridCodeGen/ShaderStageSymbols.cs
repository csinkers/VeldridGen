using Microsoft.CodeAnalysis;

namespace VeldridCodeGen
{
    public class ShaderStageSymbols
    {
        public ShaderStageSymbols(Compilation compilation)
        {
            Type = Util.Resolve(compilation, "Veldrid.ShaderStages");
            foreach (var member in Type.GetMembers())
            {
                if (member.Name == "Vertex")
                    Vertex = member;
                else if (member.Name == "Fragment")
                    Fragment = member;
            }

            if (Vertex == null) throw new TypeResolutionException("Veldrid.ShaderStages.Vertex");
            if (Fragment == null) throw new TypeResolutionException("Veldrid.ShaderStages.Fragment");
        }

        public INamedTypeSymbol Type { get; }
        public ISymbol Fragment { get; }
        public ISymbol Vertex { get; }
    }
}