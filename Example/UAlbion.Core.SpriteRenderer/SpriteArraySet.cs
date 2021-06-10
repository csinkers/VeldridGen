using UAlbion.Core.Veldrid;
using VeldridCodeGen.Interfaces;

namespace UAlbion.Core.SpriteRenderer
{
    public sealed partial class SpriteArraySet : ResourceSetHolder
    {
        [Resource("uSprite")] Texture2DArrayHolder _texture;
        [Resource("uSpriteSampler")] SamplerHolder _sampler;
        [Resource("_Uniform")] SingleBuffer<SpriteUniform> _uniform;
    }
}