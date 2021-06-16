using System;
using Veldrid;

namespace VeldridGen.Interfaces
{
    public interface IBufferHolder : IDisposable
    {
        DeviceBuffer DeviceBuffer { get; }
    }
}
