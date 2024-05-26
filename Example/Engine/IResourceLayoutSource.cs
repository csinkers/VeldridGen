using System;
using Veldrid;

namespace VeldridGen.Example.Engine;

public interface IResourceLayoutSource
{
    ResourceLayout GetLayout(Type type, GraphicsDevice device);
}