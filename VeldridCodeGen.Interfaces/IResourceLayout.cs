using System;

namespace VeldridCodeGen.Interfaces
{
    public interface IResourceLayout : IDisposable
    {
        string Name { get; set; }
    }
}