using System;

namespace VeldridGen.Interfaces
{
    public interface IPipelineHolder : IDisposable
    {
        public Veldrid.Pipeline Pipeline { get; }
        public string Name { get; set; }
    }
}
