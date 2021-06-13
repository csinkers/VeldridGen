namespace VeldridCodeGen
{
    public static class Interfaces
    {
        public const string Namespace             = "VeldridCodeGen.Interfaces";
        public const string UniformFormatName     = "IUniformFormat";
        public const string VertexFormatName      = "IVertexFormat";
        public const string ResourceSetHolderName = "IResourceSetHolder";
        public const string FramebufferHolderName = "IFramebufferHolder";
        public const string PipelineHolderName    = "IPipelineHolder";
        public const string SamplerHolderName     = "ISamplerHolder";
        public const string BufferHolderName      = "IBufferHolder";
        public const string TextureHolderName     = "ITextureHolder";

        public static readonly string UniformFormatFullName     = $"{Namespace}.{UniformFormatName}";
        public static readonly string VertexFormatFullName      = $"{Namespace}.{VertexFormatName}";
        public static readonly string ResourceSetHolderFullName = $"{Namespace}.{ResourceSetHolderName}";
        public static readonly string FramebufferHolderFullName = $"{Namespace}.{FramebufferHolderName}";
        public static readonly string PipelineHolderFullName    = $"{Namespace}.{PipelineHolderName}";
        public static readonly string SamplerHolderFullName     = $"{Namespace}.{SamplerHolderName}";
        public static readonly string BufferHolderFullName      = $"{Namespace}.{BufferHolderName}";
        public static readonly string TextureHolderFullName     = $"{Namespace}.{TextureHolderName}";

        public static string Source => $@"
using System;
using System.ComponentModel;
using Veldrid;

namespace {Namespace}
{{
    // Struct markers
    public interface {UniformFormatName} {{ }}
    public interface {VertexFormatName} {{ }}

    // Holder interfaces
    public interface {ResourceSetHolderName} : IDisposable
    {{
        ResourceSet ResourceSet {{ get; }}
    }}
    public interface {FramebufferHolderName} : IDisposable, INotifyPropertyChanged
    {{
        uint Width {{ get; set; }}
        uint Height {{ get; set; }}
        Framebuffer Framebuffer {{ get; }}
    }}
    public interface {PipelineHolderName} : IDisposable
    {{
        public Pipeline Pipeline {{ get; }}
    }}
    public interface {SamplerHolderName} : INotifyPropertyChanged, IDisposable
    {{
        Sampler Sampler {{ get; }}
    }}
    public interface {BufferHolderName} : IDisposable
    {{
        DeviceBuffer DeviceBuffer {{ get; }}
    }}
    public interface {TextureHolderName} : IDisposable, INotifyPropertyChanged
    {{
        public Texture DeviceTexture {{ get; }}
        public TextureView TextureView {{ get; }}
    }}
}}
";
    }
}
