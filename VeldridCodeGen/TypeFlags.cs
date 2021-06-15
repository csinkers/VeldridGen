using System;

namespace VeldridCodeGen
{
    [Flags]
    enum TypeFlags
    {
        IsShader = 0x300,

        IsUniformFormat = 0x1,
        IsVertexFormat = 0x2,
        IsResourceSetHolder = 0x4,
        IsFramebufferHolder = 0x8,
        IsPipelineHolder = 0x10,
        IsSamplerHolder = 0x20,
        IsBufferHolder = 0x40,
        IsTextureHolder = 0x80,
        IsVertexShader = 0x100,
        IsFragmentShader = 0x200,
    }
}