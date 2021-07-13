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
            BufferHolder      = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IBufferHolder`1");
            FragmentShader    = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IFragmentShader");
            FramebufferHolder = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IFramebufferHolder");
            PipelineHolder    = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IPipelineHolder");
            ResourceSetHolder = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IResourceSetHolder");
            SamplerHolder     = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.ISamplerHolder");
            TextureHolder     = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.ITextureHolder");
            TextureArrayHolder= VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.ITextureArrayHolder");
            UniformFormat     = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IUniformFormat");
            VertexFormat      = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IVertexFormat");
            VertexShader      = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.IVertexShader");

            ColorAttachmentAttrib = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.ColorAttachmentAttribute");
            DepthAttachmentAttrib = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.DepthAttachmentAttribute");
            VertexAttrib          = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.VertexAttribute");
            ResourceAttrib        = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.ResourceAttribute");

            FragmentShaderAttrib = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.FragmentShaderAttribute");
            InputAttrib          = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.InputAttribute");
            NameAttrib           = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.NameAttribute");
            OutputAttrib         = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.OutputAttribute");
            ResourceSetAttrib    = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.ResourceSetAttribute");
            UniformAttrib        = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.UniformAttribute");
            VertexShaderAttrib   = VeldridGenUtil.Resolve(compilation, "VeldridGen.Interfaces.VertexShaderAttribute");

            Byte = VeldridGenUtil.Resolve(compilation, typeof(byte).FullName!);
            Short = VeldridGenUtil.Resolve(compilation, typeof(short).FullName!);
            UShort = VeldridGenUtil.Resolve(compilation, typeof(ushort).FullName!);
            Int = VeldridGenUtil.Resolve(compilation, typeof(int).FullName!);
            UInt = VeldridGenUtil.Resolve(compilation, typeof(uint).FullName!);
            Float = VeldridGenUtil.Resolve(compilation, typeof(float).FullName!);
            Vector2 = VeldridGenUtil.Resolve(compilation, typeof(System.Numerics.Vector2).FullName!);
            Vector3 = VeldridGenUtil.Resolve(compilation, typeof(System.Numerics.Vector3).FullName!);
            Vector4 = VeldridGenUtil.Resolve(compilation, typeof(System.Numerics.Vector4).FullName!);
            Matrix4x4 = VeldridGenUtil.Resolve(compilation, typeof(System.Numerics.Matrix4x4).FullName!);
            NotifyPropertyChanged = VeldridGenUtil.Resolve(compilation, typeof(System.ComponentModel.INotifyPropertyChanged).FullName!);

            VertexElementFormat = new VertexElementFormatSymbols(compilation);
            ShaderStages = new ShaderStageSymbols(compilation);
            ResourceKind = new ResourceKindSymbols(compilation);
        }
    }
}
