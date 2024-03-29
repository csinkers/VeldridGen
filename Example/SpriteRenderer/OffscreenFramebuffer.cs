﻿using Veldrid;
using VeldridGen.Example.Engine;
using VeldridGen.Interfaces;

namespace VeldridGen.Example.SpriteRenderer
{
    public partial class OffscreenFramebuffer : FramebufferHolder
    {
        [DepthAttachment(PixelFormat.R32_Float)] Texture _depth; 
        [ColorAttachment(PixelFormat.B8_G8_R8_A8_UNorm)] Texture _color; 
        public OffscreenFramebuffer(uint width, uint height) : base(width, height) { }
    }
}

