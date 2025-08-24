using System;

namespace VeldridGen.Interfaces;

/// <summary>
/// The interface for objects that hold a <see cref="Pipeline"/>.
/// </summary>
public interface IPipelineHolder : IDisposable
{
    public string Name { get; set; }
    public Veldrid.Pipeline Pipeline { get; }
}
