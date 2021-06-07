using System;
using Veldrid;

namespace VeldridCodeGen.Interfaces
{
    public interface IBufferHolder : IDisposable
    {
        DeviceBuffer DeviceBuffer { get; }
    }
}