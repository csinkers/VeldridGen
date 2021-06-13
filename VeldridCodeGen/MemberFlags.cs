using System;

namespace VeldridCodeGen
{
    [Flags]
    enum MemberFlags
    {
        IsProperty = 0x1,
        IsColorAttachment = 0x2,
        IsDepthAttachment = 0x4,
        IsVertexComponent = 0x8,
        IsResource = 0x10,
    }
}