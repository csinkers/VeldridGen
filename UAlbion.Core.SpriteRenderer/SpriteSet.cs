using UAlbion.Core.Veldrid;
using Veldrid;
using VeldridCodeGen.Interfaces;

namespace UAlbion.Core.SpriteRenderer
{
    public partial class SpriteSet : Component, IResourceLayout
    {
        [Resource("uSprite", ShaderStages.Fragment)] Texture2DArrayHolder _texture;
        [Resource("uSpriteSampler", ShaderStages.Fragment)] SamplerHolder _sampler;
        [Resource("_Uniform")] SingleBuffer<SpriteUniform> _uniform;
    }
}
