using Veldrid;

namespace UAlbion.Core
{
    public interface IShaderSource
    {
        string GetGlsl(string shaderName);
        Shader[] GetShaderPair(ResourceFactory resourceFactory, string vertexShaderName, string fragmentShaderName, string vertexShaderContent, string fragmentShaderContent);
    }
}