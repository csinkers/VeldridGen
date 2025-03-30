using System;
using Veldrid;

namespace VeldridGen.Interfaces;

public class DepthAttachmentAttribute(PixelFormat format) : Attribute
{
    public PixelFormat Format { get; } = format;
}
