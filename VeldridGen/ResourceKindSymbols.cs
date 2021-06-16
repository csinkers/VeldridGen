using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    public class ResourceKindSymbols
    {
        public ResourceKindSymbols(Compilation compilation)
        {
            Type = Util.Resolve(compilation, "Veldrid.ResourceKind");
            foreach (var member in Type.GetMembers())
            {
                if (member.Name == "UniformBuffer") UniformBuffer = member;
                else if (member.Name == "TextureReadOnly") TextureReadOnly = member;
                else if (member.Name == "Sampler") Sampler = member;
            }

            if (UniformBuffer == null) throw new TypeResolutionException("Veldrid.ResourceKind.UniformBuffer");
            if (TextureReadOnly == null) throw new TypeResolutionException("Veldrid.ResourceKind.TextureReadOnly");
            if (Sampler == null) throw new TypeResolutionException("Veldrid.ResourceKind.Sampler");
        }

        public INamedTypeSymbol Type { get; }
        public ISymbol Sampler { get; }
        public ISymbol TextureReadOnly { get; }
        public ISymbol UniformBuffer { get; }
    }
}
