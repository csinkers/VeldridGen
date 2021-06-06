using System;
using Veldrid;

namespace UAlbion.Core
{
    public interface IResourceLayoutSource
    {
        ResourceLayout Get(Type type, GraphicsDevice device);
    }
}