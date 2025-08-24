using System;
using Veldrid;

namespace VeldridGen.Interfaces;

/// <summary>
/// Applied to <see cref="ITextureHolder"/> members of an <see cref="IFramebufferHolder"/> to indicate that the texture is a depth attachment.
/// </summary>
/// <param name="format">The pixel format of the frame-buffer's depth attachment</param>
public class DepthAttachmentAttribute(PixelFormat format) : Attribute
{
    public PixelFormat Format { get; } = format;
}
