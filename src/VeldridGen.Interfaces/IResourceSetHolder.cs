using System;
using Veldrid;

namespace VeldridGen.Interfaces;

/// <summary>
/// The interface for classes that hold a <see cref="ResourceSet"/> and manage its lifetime.
/// </summary>
public interface IResourceSetHolder : IDisposable
{
    ResourceSet ResourceSet { get; }
}
