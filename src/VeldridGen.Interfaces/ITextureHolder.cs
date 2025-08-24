using System.ComponentModel;
using Veldrid;

namespace VeldridGen.Interfaces;

/// <summary>
/// The interface for classes that hold a <see cref="Texture"/> and manage its lifetime.
/// </summary>
public interface ITextureHolder : INotifyPropertyChanged
{
    Texture DeviceTexture { get; }
    string Name { get; }
}
