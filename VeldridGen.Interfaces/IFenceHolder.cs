using System;
using System.ComponentModel;
using Veldrid;

namespace VeldridGen.Interfaces
{
    public interface IFenceHolder : IDisposable, INotifyPropertyChanged
    {
        string Name { get; }
        Fence Fence { get; }
    }
}