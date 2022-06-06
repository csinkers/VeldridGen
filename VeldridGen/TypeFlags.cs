using System;

namespace VeldridGen
{
    [Flags]
    public enum TypeFlags
    {
        IsShader = IsVertexShader | IsFragmentShader,

        IsUniformFormat      = 0x1,
        IsStructuredFormat   = 0x2,
        IsVertexFormat       = 0x4,
        IsResourceSetHolder  = 0x8,
        IsFramebufferHolder  = 0x10,
        IsPipelineHolder     = 0x20,
        IsSamplerHolder      = 0x40,
        IsBufferHolder       = 0x80,
        IsTextureHolder      = 0x100,
        IsTextureArrayHolder = 0x200,
        IsVertexShader       = 0x400,
        IsFragmentShader     = 0x800,
    }
}
