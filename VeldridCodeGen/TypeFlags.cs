using System;

namespace VeldridCodeGen
{
    [Flags]
    enum TypeFlags
    {
        IsUniformFormat = 0x1,
        IsVertexFormat = 0x2,
        IsResourceSetHolder = 0x4,
        IsFramebufferHolder = 0x8,
        IsPipelineHolder = 0x10,
        IsSamplerHolder = 0x20,
        IsBufferHolder = 0x40,
        IsTextureHolder = 0x80,
    }
}