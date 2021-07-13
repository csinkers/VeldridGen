using System;

namespace VeldridGen
{
    [Flags]
    public enum TypeFlags
    {
        IsShader = IsVertexShader | IsFragmentShader,

        IsUniformFormat      = 0x1,
        IsVertexFormat       = 0x2,
        IsResourceSetHolder  = 0x4,
        IsFramebufferHolder  = 0x8,
        IsPipelineHolder     = 0x10,
        IsSamplerHolder      = 0x20,
        IsBufferHolder       = 0x40,
        IsTextureHolder      = 0x80,
        IsTextureArrayHolder = 0x100,
        IsVertexShader       = 0x200,
        IsFragmentShader     = 0x400,
    }
}
