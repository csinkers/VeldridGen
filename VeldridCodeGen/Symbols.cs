using Microsoft.CodeAnalysis;

namespace VeldridCodeGen
{
    public class Symbols
    {
        // Interfaces
        public INamedTypeSymbol UniformFormat { get; }
        public INamedTypeSymbol VertexFormat { get; }
        public INamedTypeSymbol ResourceSetHolder { get; }
        public INamedTypeSymbol FramebufferHolder { get; }
        public INamedTypeSymbol PipelineHolder { get; }
        public INamedTypeSymbol SamplerHolder { get; }
        public INamedTypeSymbol BufferHolder { get; }
        public INamedTypeSymbol TextureHolder { get; }

        // Attributes
        public INamedTypeSymbol ColorAttachmentAttrib { get; }
        public INamedTypeSymbol DepthAttachmentAttrib { get; }
        public INamedTypeSymbol InputParamAttrib { get; }
        public INamedTypeSymbol ResourceAttrib { get; }

        // Built-in types
        public INamedTypeSymbol Int { get; }
        public INamedTypeSymbol UInt { get; }
        public INamedTypeSymbol Float { get; }
        public INamedTypeSymbol Vector2 { get; }
        public INamedTypeSymbol Vector3 { get; }
        public INamedTypeSymbol Vector4 { get; }
        public INamedTypeSymbol NotifyPropertyChanged { get; }

        public Symbols(Compilation compilation)
        {
            UniformFormat = compilation.GetTypeByMetadataName(Interfaces.UniformFormatFullName);
            VertexFormat = compilation.GetTypeByMetadataName(Interfaces.VertexFormatFullName);
            ResourceSetHolder = compilation.GetTypeByMetadataName(Interfaces.ResourceSetHolderFullName);
            FramebufferHolder = compilation.GetTypeByMetadataName(Interfaces.FramebufferHolderFullName);
            PipelineHolder = compilation.GetTypeByMetadataName(Interfaces.PipelineHolderFullName);
            SamplerHolder = compilation.GetTypeByMetadataName(Interfaces.SamplerHolderFullName);
            BufferHolder = compilation.GetTypeByMetadataName(Interfaces.BufferHolderFullName);
            TextureHolder = compilation.GetTypeByMetadataName(Interfaces.TextureHolderFullName);

            ColorAttachmentAttrib = compilation.GetTypeByMetadataName(Attributes.ColorAttachmentFullName);
            DepthAttachmentAttrib = compilation.GetTypeByMetadataName(Attributes.DepthAttachmentFullName);
            InputParamAttrib = compilation.GetTypeByMetadataName(Attributes.InputParamFullName);
            ResourceAttrib = compilation.GetTypeByMetadataName(Attributes.ResourceFullName);

            Int = compilation.GetTypeByMetadataName("System.Int32");
            UInt = compilation.GetTypeByMetadataName("System.UInt32");
            Float = compilation.GetTypeByMetadataName("System.Single");
            Vector2 = compilation.GetTypeByMetadataName("System.Numerics.Vector2");
            Vector3 = compilation.GetTypeByMetadataName("System.Numerics.Vector3");
            Vector4 = compilation.GetTypeByMetadataName("System.Numerics.Vector4");
            NotifyPropertyChanged = compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged");
        }
    }
}