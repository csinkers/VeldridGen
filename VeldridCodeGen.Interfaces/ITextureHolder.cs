using System;
using System.ComponentModel;
using Veldrid;

namespace VeldridCodeGen.Interfaces
{
    public interface ITextureHolder : IDisposable, INotifyPropertyChanged
    {
        public Texture DeviceTexture { get; }
        public TextureView TextureView { get; }
    }
}