using System;

namespace VeldridCodeGen.Interfaces
{
    public interface IPipelineHolder : IDisposable
    {
        public Veldrid.Pipeline Pipeline { get; }
        public string Name { get; set; }
    }
}