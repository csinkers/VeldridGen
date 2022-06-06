using Microsoft.CodeAnalysis;

namespace VeldridGen.Symbols
{
    public class InterfaceSymbols
    {
        // Interfaces
        public INamedTypeSymbol BufferHolder { get; }
        public INamedTypeSymbol FragmentShader { get; }
        public INamedTypeSymbol FramebufferHolder { get; }
        public INamedTypeSymbol PipelineHolder { get; }
        public INamedTypeSymbol ResourceSetHolder { get; }
        public INamedTypeSymbol SamplerHolder { get; }
        public INamedTypeSymbol StructuredFormat { get; }
        public INamedTypeSymbol TextureHolder { get; }
        public INamedTypeSymbol TextureArrayHolder { get; }
        public INamedTypeSymbol UniformFormat { get; }
        public INamedTypeSymbol VertexFormat { get; }
        public INamedTypeSymbol VertexShader { get; }

        public InterfaceSymbols(Compilation compilation)
        {
            BufferHolder      = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IBufferHolder`1");
            FragmentShader    = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IFragmentShader");
            FramebufferHolder = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IFramebufferHolder");
            PipelineHolder    = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IPipelineHolder");
            ResourceSetHolder = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IResourceSetHolder");
            SamplerHolder     = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.ISamplerHolder");
            StructuredFormat  = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IStructuredFormat");
            TextureHolder     = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.ITextureHolder");
            TextureArrayHolder= VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.ITextureArrayHolder");
            UniformFormat     = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IUniformFormat");
            VertexFormat      = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IVertexFormat");
            VertexShader      = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IVertexShader");
        }
    }
}