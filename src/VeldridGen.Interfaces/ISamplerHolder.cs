using System;
using System.ComponentModel;
using Veldrid;

namespace VeldridGen.Interfaces;

/// <summary>
/// The interface for classes that hold a <see cref="Sampler"/> and manage its lifetime.
/// </summary>
public interface ISamplerHolder : INotifyPropertyChanged, IDisposable
{
    Sampler Sampler { get; }
}
