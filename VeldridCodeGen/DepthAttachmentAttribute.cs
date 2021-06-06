using System;
using Veldrid;

namespace UAlbion.Core
{
    public class DepthAttachmentAttribute : Attribute
    {
        public PixelFormat Format { get; }
        public DepthAttachmentAttribute(PixelFormat format) => Format = format;
    }
}