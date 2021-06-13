using System;
using Veldrid;

namespace UAlbion.Core.Veldrid
{
    public interface IShaderCache
    {
        event EventHandler<EventArgs> ShadersUpdated;
        string GetGlsl(string shaderName);
        void CleanupOldFiles();
        void DestroyAllDeviceObjects();
        IShaderCache AddShaderPath(string path);
        Shader[] GetShaderPair(ResourceFactory factory,
            string vertexShaderName, string fragmentShaderName,
            string vertexShaderContent = null, string fragmentShaderContent = null);
    }
}