using System;
using Veldrid;

namespace UAlbion.Core
{
    public interface IShaderCache : IComponent
    {
        event EventHandler<EventArgs> ShadersUpdated;
        string GetGlsl(string shaderName);
        void CleanupOldFiles();
        void DestroyAllDeviceObjects();
        IShaderCache AddShaderPath(string path);
    }

    public interface IVeldridShaderCache : IShaderCache
    {
        Shader[] GetShaderPair(ResourceFactory factory,
            string vertexShaderName, string fragmentShaderName,
            string vertexShaderContent = null, string fragmentShaderContent = null);
    }
}
