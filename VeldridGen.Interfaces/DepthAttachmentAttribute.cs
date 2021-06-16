using System;
using Veldrid;

namespace VeldridGen.Interfaces
{
    public class DepthAttachmentAttribute : Attribute
    {
        public PixelFormat Format { get; }
        public DepthAttachmentAttribute(PixelFormat format) => Format = format;
    }
}
