using System;
using System.ComponentModel;
using Veldrid;

namespace VeldridGen.Interfaces
{
    public interface ITextureArrayHolder : IDisposable, INotifyPropertyChanged
    {
        Texture DeviceTexture { get; }
        string Name { get; }
    }
}