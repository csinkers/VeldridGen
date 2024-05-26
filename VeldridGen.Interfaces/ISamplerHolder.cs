using System;
using System.ComponentModel;
using Veldrid;

namespace VeldridGen.Interfaces;

public interface ISamplerHolder : INotifyPropertyChanged, IDisposable
{
    Sampler Sampler { get; }
}
