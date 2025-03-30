using System;
using Veldrid;

namespace VeldridGen.Interfaces;

public class ColorAttachmentAttribute(PixelFormat format) : Attribute
{
    public PixelFormat Format { get; } = format;
}
