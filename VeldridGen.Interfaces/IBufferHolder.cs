using System;
using Veldrid;

namespace VeldridGen.Interfaces
{
    public interface IBufferHolder<T> : IDisposable
    {
        DeviceBuffer DeviceBuffer { get; }
    }
}
