using VeldridGen.Interfaces;

namespace VeldridGen.Example.Engine.Visual;

public interface ITextureSource
{
    ITextureHolder GetSimpleTexture(ITexture texture);
    ITextureArrayHolder GetArrayTexture(ITexture texture);
}
