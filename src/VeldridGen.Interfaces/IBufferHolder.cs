using System;
using Veldrid;

namespace VeldridGen.Interfaces;

/// <summary>
/// The base interface for types that hold a <see cref="DeviceBuffer"/> and manage its lifetime.
/// </summary>
/// <typeparam name="T">The uniform struct type that the buffer will hold</typeparam>
public interface IBufferHolder<T> : IDisposable
{
    DeviceBuffer DeviceBuffer { get; }
}
