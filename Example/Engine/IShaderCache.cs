using System;
using Veldrid;

namespace VeldridGen.Example.Engine
{
    public interface IShaderCache
    {
        event EventHandler<EventArgs> ShadersUpdated;
        string GetGlsl(string shaderName);
        void CleanupOldFiles();
        IShaderCache AddShaderPath(string path);
        Shader[] GetShaderPair(ResourceFactory factory,
            string vertexShaderName, string fragmentShaderName,
            string vertexShaderContent = null, string fragmentShaderContent = null);
    }
}