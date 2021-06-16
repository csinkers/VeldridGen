using System;

namespace VeldridGen
{
    [Flags]
    enum MemberFlags
    {
        IsColorAttachment = 0x1,
        IsDepthAttachment = 0x2,
        IsVertexComponent = 0x4,
        IsResource = 0x8,
        IsUniform = 0x10,
    }
}
