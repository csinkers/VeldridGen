using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    public class Symbols
    {
        // Interfaces
        public INamedTypeSymbol BufferHolder { get; }
        public INamedTypeSymbol FragmentShader { get; }
        public INamedTypeSymbol FramebufferHolder { get; }
        public INamedTypeSymbol PipelineHolder { get; }
        public INamedTypeSymbol ResourceSetHolder { get; }
        public INamedTypeSymbol SamplerHolder { get; }
        public INamedTypeSymbol TextureHolder { get; }
        public INamedTypeSymbol TextureArrayHolder { get; }
        public INamedTypeSymbol UniformFormat { get; }
        public INamedTypeSymbol VertexFormat { get; }
        public INamedTypeSymbol VertexShader { get; }

        // Attributes
        public INamedTypeSymbol ColorAttachmentAttrib { get; }
        public INamedTypeSymbol DepthAttachmentAttrib { get; }
        public INamedTypeSymbol FragmentShaderAttrib { get; }
        public INamedTypeSymbol InputAttrib { get; }
        public INamedTypeSymbol NameAttrib { get; }
        public INamedTypeSymbol OutputAttrib { get; }
        public INamedTypeSymbol ResourceAttrib { get; }
        public INamedTypeSymbol ResourceSetAttrib { get; }
        public INamedTypeSymbol UniformAttrib { get; }
        public INamedTypeSymbol VertexAttrib { get; }
        public INamedTypeSymbol VertexShaderAttrib { get; }

        // Built-in types
        public INamedTypeSymbol Byte { get; }
        public INamedTypeSymbol Short { get; }
        public INamedTypeSymbol UShort { get; }
        public INamedTypeSymbol Int { get; }
        public INamedTypeSymbol UInt { get; }
        public INamedTypeSymbol Float { get; }
        public INamedTypeSymbol Vector2 { get; }
        public INamedTypeSymbol Vector3 { get; }
        public INamedTypeSymbol Vector4 { get; }
        public INamedTypeSymbol Matrix4x4 { get; }
        public INamedTypeSymbol NotifyPropertyChanged { get; }
        public VertexElementFormatSymbols VertexElementFormat { get; }
        public ShaderStageSymbols ShaderStages { get; }
        public ResourceKindSymbols ResourceKind { get; }

        public Symbols(Compilation compilation)
        {
            BufferHolder      = Util.Resolve(compilation, "VeldridGen.Interfaces.IBufferHolder`1");
            FragmentShader    = Util.Resolve(compilation, "VeldridGen.Interfaces.IFragmentShader");
            FramebufferHolder = Util.Resolve(compilation, "VeldridGen.Interfaces.IFramebufferHolder");
            PipelineHolder    = Util.Resolve(compilation, "VeldridGen.Interfaces.IPipelineHolder");
            ResourceSetHolder = Util.Resolve(compilation, "VeldridGen.Interfaces.IResourceSetHolder");
            SamplerHolder     = Util.Resolve(compilation, "VeldridGen.Interfaces.ISamplerHolder");
            TextureHolder     = Util.Resolve(compilation, "VeldridGen.Interfaces.ITextureHolder");
            TextureArrayHolder= Util.Resolve(compilation, "VeldridGen.Interfaces.ITextureArrayHolder");
            UniformFormat     = Util.Resolve(compilation, "VeldridGen.Interfaces.IUniformFormat");
            VertexFormat      = Util.Resolve(compilation, "VeldridGen.Interfaces.IVertexFormat");
            VertexShader      = Util.Resolve(compilation, "VeldridGen.Interfaces.IVertexShader");

            ColorAttachmentAttrib = Util.Resolve(compilation, "VeldridGen.Interfaces.ColorAttachmentAttribute");
            DepthAttachmentAttrib = Util.Resolve(compilation, "VeldridGen.Interfaces.DepthAttachmentAttribute");
            VertexAttrib          = Util.Resolve(compilation, "VeldridGen.Interfaces.VertexAttribute");
            ResourceAttrib        = Util.Resolve(compilation, "VeldridGen.Interfaces.ResourceAttribute");

            FragmentShaderAttrib = Util.Resolve(compilation, "VeldridGen.Interfaces.FragmentShaderAttribute");
            InputAttrib          = Util.Resolve(compilation, "VeldridGen.Interfaces.InputAttribute");
            NameAttrib           = Util.Resolve(compilation, "VeldridGen.Interfaces.NameAttribute");
            OutputAttrib         = Util.Resolve(compilation, "VeldridGen.Interfaces.OutputAttribute");
            ResourceSetAttrib    = Util.Resolve(compilation, "VeldridGen.Interfaces.ResourceSetAttribute");
            UniformAttrib        = Util.Resolve(compilation, "VeldridGen.Interfaces.UniformAttribute");
            VertexShaderAttrib   = Util.Resolve(compilation, "VeldridGen.Interfaces.VertexShaderAttribute");

            Byte = Util.Resolve(compilation, typeof(byte).FullName!);
            Short = Util.Resolve(compilation, typeof(short).FullName!);
            UShort = Util.Resolve(compilation, typeof(ushort).FullName!);
            Int = Util.Resolve(compilation, typeof(int).FullName!);
            UInt = Util.Resolve(compilation, typeof(uint).FullName!);
            Float = Util.Resolve(compilation, typeof(float).FullName!);
            Vector2 = Util.Resolve(compilation, typeof(System.Numerics.Vector2).FullName!);
            Vector3 = Util.Resolve(compilation, typeof(System.Numerics.Vector3).FullName!);
            Vector4 = Util.Resolve(compilation, typeof(System.Numerics.Vector4).FullName!);
            Matrix4x4 = Util.Resolve(compilation, typeof(System.Numerics.Matrix4x4).FullName!);
            NotifyPropertyChanged = Util.Resolve(compilation, typeof(System.ComponentModel.INotifyPropertyChanged).FullName!);

            VertexElementFormat = new VertexElementFormatSymbols(compilation);
            ShaderStages = new ShaderStageSymbols(compilation);
            ResourceKind = new ResourceKindSymbols(compilation);
        }
    }
}
