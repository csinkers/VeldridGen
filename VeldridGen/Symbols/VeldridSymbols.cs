using Microsoft.CodeAnalysis;

namespace VeldridGen.Symbols
{
    public class VeldridSymbols
    {
        public VertexElementFormatSymbols VertexElementFormat { get; }
        public ShaderStageSymbols ShaderStages { get; }
        public ResourceKindSymbols ResourceKind { get; }

        public VeldridSymbols(Compilation compilation)
        {
            VertexElementFormat = new VertexElementFormatSymbols(compilation);
            ShaderStages = new ShaderStageSymbols(compilation);
            ResourceKind = new ResourceKindSymbols(compilation);
        }

    }
}