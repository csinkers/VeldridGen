﻿using System;
using System.ComponentModel;
using Veldrid;

namespace VeldridGen.Interfaces
{
    public interface IFramebufferHolder : IDisposable, INotifyPropertyChanged
    {
        uint Width { get; set; }
        uint Height { get; set; }
        Framebuffer Framebuffer { get; }
        OutputDescription? OutputDescription { get; }
    }
}
