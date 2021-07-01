using System.ComponentModel;
using Veldrid;

namespace VeldridGen.Interfaces
{
    public interface ITextureHolder : INotifyPropertyChanged
    {
        Texture DeviceTexture { get; }
        string Name { get; }
    }
}
