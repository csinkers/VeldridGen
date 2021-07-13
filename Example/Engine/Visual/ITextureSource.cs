using VeldridGen.Interfaces;

namespace VeldridGen.Example.Engine.Visual
{
    public interface ITextureSource
    {
        ITextureHolder GetSimpleTexture(ITexture texture, int version = 0);
        ITextureArrayHolder GetArrayTexture(ITexture texture, int version = 0);
    }
}