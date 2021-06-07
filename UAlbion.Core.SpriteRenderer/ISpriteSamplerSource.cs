using UAlbion.Core.Sprites;
using UAlbion.Core.Veldrid;

namespace UAlbion.Core.SpriteRenderer
{
    public interface ISpriteSamplerSource
    {
        SamplerHolder Get(SpriteSampler sampler);
    }
}