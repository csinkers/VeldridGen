using Microsoft.CodeAnalysis;

namespace VeldridGen.Symbols;

public class VeldridSymbols(Compilation compilation)
{
    public VertexElementFormatSymbols VertexElementFormat { get; } = new(compilation);
    public ShaderStageSymbols ShaderStages { get; } = new(compilation);
    public ResourceKindSymbols ResourceKind { get; } = new(compilation);
}
