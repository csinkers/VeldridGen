using System;
using Veldrid;

namespace UAlbion.Core
{
    public class ColorAttachmentAttribute : Attribute
    {
        public PixelFormat Format { get; }
        public ColorAttachmentAttribute(PixelFormat format) => Format = format;
    }
}