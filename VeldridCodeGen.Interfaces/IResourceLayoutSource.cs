using System;
using Veldrid;

namespace VeldridCodeGen.Interfaces
{
    public interface IResourceLayoutSource
    {
        ResourceLayout Get(Type type, GraphicsDevice device);
    }
}