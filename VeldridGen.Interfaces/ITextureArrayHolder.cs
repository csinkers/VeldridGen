using System.ComponentModel;
using Veldrid;

namespace VeldridGen.Interfaces;

public interface ITextureArrayHolder : INotifyPropertyChanged
{
    Texture DeviceTexture { get; }
    string Name { get; }
}
