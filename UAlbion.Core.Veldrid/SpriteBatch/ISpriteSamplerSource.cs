using UAlbion.Core.Sprites;

namespace UAlbion.Core.Veldrid.SpriteBatch
{
    public interface ISpriteSamplerSource
    {
        SamplerHolder Get(SpriteSampler sampler);
    }
}