using Microsoft.CodeAnalysis;

namespace VeldridGen.Symbols;

public class InterfaceSymbols(Compilation compilation)
{
    // Interfaces
    public INamedTypeSymbol BufferHolder { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IBufferHolder`1");
    public INamedTypeSymbol FragmentShader { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IFragmentShader");
    public INamedTypeSymbol FramebufferHolder { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IFramebufferHolder");
    public INamedTypeSymbol PipelineHolder { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IPipelineHolder");
    public INamedTypeSymbol ResourceSetHolder { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IResourceSetHolder");
    public INamedTypeSymbol SamplerHolder { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.ISamplerHolder");
    public INamedTypeSymbol StructuredFormat { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IStructuredFormat");
    public INamedTypeSymbol TextureHolder { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.ITextureHolder");
    public INamedTypeSymbol TextureArrayHolder { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.ITextureArrayHolder");
    public INamedTypeSymbol UniformFormat { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IUniformFormat");
    public INamedTypeSymbol VertexFormat { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IVertexFormat");
    public INamedTypeSymbol VertexShader { get; } = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IVertexShader");
}
