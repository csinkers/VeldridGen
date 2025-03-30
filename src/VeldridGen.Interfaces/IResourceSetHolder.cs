using System;
using Veldrid;

namespace VeldridGen.Interfaces;

public interface IResourceSetHolder : IDisposable
{
    ResourceSet ResourceSet { get; }
}
