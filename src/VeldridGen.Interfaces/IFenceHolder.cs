using System;
using System.ComponentModel;
using Veldrid;

namespace VeldridGen.Interfaces;

/// <summary>
/// The interface for objects that hold a <see cref="Fence"/> and manage its lifetime.
/// </summary>
public interface IFenceHolder : IDisposable, INotifyPropertyChanged
{
    string Name { get; }
    Fence Fence { get; }
}
