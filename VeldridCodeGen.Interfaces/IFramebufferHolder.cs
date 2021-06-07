using System;
using System.ComponentModel;
using Veldrid;

namespace VeldridCodeGen.Interfaces
{
    public interface IFramebufferHolder : IDisposable, INotifyPropertyChanged
    {
        uint Width { get; set; }
        uint Height { get; set; }
        Framebuffer Framebuffer { get; }
    }
}