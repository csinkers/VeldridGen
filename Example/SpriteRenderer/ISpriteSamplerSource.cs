using VeldridGen.Example.Engine.Visual.Sprites;
using VeldridGen.Interfaces;

namespace VeldridGen.Example.SpriteRenderer
{
    public interface ISpriteSamplerSource
    {
        ISamplerHolder GetSampler(SpriteSampler sampler);
    }
}
