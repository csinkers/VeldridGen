using Microsoft.CodeAnalysis;

namespace VeldridCodeGen
{
    public class Symbols
    {
        // Interfaces
        public INamedTypeSymbol BufferHolder { get; }
        public INamedTypeSymbol FramebufferHolder { get; }
        public INamedTypeSymbol PipelineHolder { get; }
        public INamedTypeSymbol ResourceSetHolder { get; }
        public INamedTypeSymbol SamplerHolder { get; }
        public INamedTypeSymbol TextureHolder { get; }
        public INamedTypeSymbol UniformFormat { get; }
        public INamedTypeSymbol VertexFormat { get; }

        // Attributes
        public INamedTypeSymbol ColorAttachmentAttrib { get; }
        public INamedTypeSymbol DepthAttachmentAttrib { get; }
        public INamedTypeSymbol VertexAttrib { get; }
        public INamedTypeSymbol ResourceAttrib { get; }

        // Built-in types
        public INamedTypeSymbol Int { get; }
        public INamedTypeSymbol UInt { get; }
        public INamedTypeSymbol Float { get; }
        public INamedTypeSymbol Vector2 { get; }
        public INamedTypeSymbol Vector3 { get; }
        public INamedTypeSymbol Vector4 { get; }
        public INamedTypeSymbol NotifyPropertyChanged { get; }
        public VertexElementFormatSymbols VertexElementFormat { get; }
        public ShaderStageSymbols ShaderStages { get; }
        public ResourceKindSymbols ResourceKind { get; }

        public Symbols(Compilation compilation)
        {
            BufferHolder      = Util.Resolve(compilation, "VeldridCodeGen.Interfaces.IBufferHolder");
            FramebufferHolder = Util.Resolve(compilation, "VeldridCodeGen.Interfaces.IFramebufferHolder");
            PipelineHolder    = Util.Resolve(compilation, "VeldridCodeGen.Interfaces.IPipelineHolder");
            ResourceSetHolder = Util.Resolve(compilation, "VeldridCodeGen.Interfaces.IResourceSetHolder");
            SamplerHolder     = Util.Resolve(compilation, "VeldridCodeGen.Interfaces.ISamplerHolder");
            TextureHolder     = Util.Resolve(compilation, "VeldridCodeGen.Interfaces.ITextureHolder");
            UniformFormat     = Util.Resolve(compilation, "VeldridCodeGen.Interfaces.IUniformFormat");
            VertexFormat      = Util.Resolve(compilation, "VeldridCodeGen.Interfaces.IVertexFormat");

            ColorAttachmentAttrib = Util.Resolve(compilation, "VeldridCodeGen.Interfaces.ColorAttachmentAttribute");
            DepthAttachmentAttrib = Util.Resolve(compilation, "VeldridCodeGen.Interfaces.DepthAttachmentAttribute");
            VertexAttrib          = Util.Resolve(compilation, "VeldridCodeGen.Interfaces.VertexAttribute");
            ResourceAttrib        = Util.Resolve(compilation, "VeldridCodeGen.Interfaces.ResourceAttribute");

            Int = Util.Resolve(compilation, typeof(int).FullName!);
            UInt = Util.Resolve(compilation, typeof(uint).FullName!);
            Float = Util.Resolve(compilation, typeof(float).FullName!);
            Vector2 = Util.Resolve(compilation, typeof(System.Numerics.Vector2).FullName!);
            Vector3 = Util.Resolve(compilation, typeof(System.Numerics.Vector3).FullName!);
            Vector4 = Util.Resolve(compilation, typeof(System.Numerics.Vector4).FullName!);
            NotifyPropertyChanged = Util.Resolve(compilation, typeof(System.ComponentModel.INotifyPropertyChanged).FullName!);

            VertexElementFormat = new VertexElementFormatSymbols(compilation);
            ShaderStages = new ShaderStageSymbols(compilation);
            ResourceKind = new ResourceKindSymbols(compilation);
        }
    }
}
