using System;
using System.ComponentModel;
using Veldrid;

namespace VeldridGen.Interfaces
{
    public interface ITextureHolder : IDisposable, INotifyPropertyChanged
    {
        public Texture DeviceTexture { get; }
    }
}
