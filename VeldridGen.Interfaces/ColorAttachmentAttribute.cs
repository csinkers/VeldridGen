using System;
using Veldrid;

namespace VeldridGen.Interfaces;

public class ColorAttachmentAttribute : Attribute
{
    public PixelFormat Format { get; }
    public ColorAttachmentAttribute(PixelFormat format) => Format = format;
}