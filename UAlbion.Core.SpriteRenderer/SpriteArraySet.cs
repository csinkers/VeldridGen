using UAlbion.Core.Veldrid;
using VeldridCodeGen.Interfaces;

namespace UAlbion.Core.SpriteRenderer
{
    public partial class SpriteArraySet : Component, IResourceLayout
    {
        [Resource("uSprite")] Texture2DArrayHolder _texture;
        [Resource("uSpriteSampler")] SamplerHolder _sampler;
        [Resource("_Uniform")] SingleBuffer<SpriteUniform> _uniform;
    }
}