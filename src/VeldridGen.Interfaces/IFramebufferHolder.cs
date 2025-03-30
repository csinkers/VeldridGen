using System;
using System.ComponentModel;
using Veldrid;

namespace VeldridGen.Interfaces;

public interface IFramebufferHolder : IDisposable, INotifyPropertyChanged
{
    string Name { get; }
    uint Width { get; set; }
    uint Height { get; set; }
    Framebuffer Framebuffer { get; }
    OutputDescription? OutputDescription { get; }
    ITextureHolder DepthTexture { get; }
    ITextureHolder GetColorTexture(int index);
}
