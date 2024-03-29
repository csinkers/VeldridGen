﻿using System;
using Veldrid;

namespace VeldridGen.Example.Engine
{
    public class MainFramebuffer : FramebufferHolder
    {
        public MainFramebuffer() : base(0, 0) { }
        protected override Framebuffer CreateFramebuffer(GraphicsDevice device)
        {
            if (device == null) throw new ArgumentNullException(nameof(device));
            return device.SwapchainFramebuffer;
        }

        public override OutputDescription? OutputDescription => Framebuffer?.OutputDescription;
    }
}
