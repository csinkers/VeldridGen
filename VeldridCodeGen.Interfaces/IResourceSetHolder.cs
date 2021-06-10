using System;
using Veldrid;

namespace VeldridCodeGen.Interfaces
{
    public interface IResourceSetHolder : IDisposable
    {
        ResourceSet ResourceSet { get; }
    }
}