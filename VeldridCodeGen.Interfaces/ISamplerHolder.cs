using System;
using System.ComponentModel;
using Veldrid;

namespace VeldridCodeGen.Interfaces
{
    public interface ISamplerHolder : INotifyPropertyChanged, IDisposable
    {
        Sampler Sampler { get; }
    }
}