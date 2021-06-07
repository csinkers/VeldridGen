using System;
using Veldrid;

namespace VeldridCodeGen.Interfaces
{
    public class DepthAttachmentAttribute : Attribute
    {
        public PixelFormat Format { get; }
        public DepthAttachmentAttribute(PixelFormat format) => Format = format;
    }
}