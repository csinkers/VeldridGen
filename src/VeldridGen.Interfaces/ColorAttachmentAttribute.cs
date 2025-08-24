using System;
using Veldrid;

namespace VeldridGen.Interfaces;

/// <summary>
/// Applied to <see cref="ITextureHolder"/> members of an <see cref="IFramebufferHolder"/> to indicate that the texture is a color attachment.
/// </summary>
/// <param name="format">The pixel format of the framebuffer attachment</param>
public class ColorAttachmentAttribute(PixelFormat format) : Attribute
{
    public PixelFormat Format { get; } = format;
}
