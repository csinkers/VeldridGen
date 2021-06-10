using UAlbion.Core.Veldrid;
using Veldrid;

namespace UAlbion.Core.SpriteRenderer
{
    // To be generated
    public partial class SpriteSet
    {
        public static readonly ResourceLayoutDescription Layout = new(
            new ResourceLayoutElementDescription("uSprite", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
            new ResourceLayoutElementDescription("uSpriteSampler", ResourceKind.Sampler, ShaderStages.Fragment),
            new ResourceLayoutElementDescription("_Uniform", ResourceKind.UniformBuffer, ShaderStages.Vertex | ShaderStages.Fragment));

        public Texture2DArrayHolder Texture
        {
            get => _texture;
            set
            {
                if (_texture == value)
                    return;

                if (_texture != null)
                    _texture.PropertyChanged -= PropertyDirty;

                _texture = value;
                
                if(_texture != null)
                    _texture.PropertyChanged += PropertyDirty;
                Dirty();
            }
        }

        public SamplerHolder Sampler
        {
            get => _sampler;
            set
            {
                if (_sampler == value)
                    return;

                if (_sampler != null)
                    _sampler.PropertyChanged -= PropertyDirty;

                _sampler = value;

                if (_sampler != null)
                    _sampler.PropertyChanged += PropertyDirty;
                Dirty();
            }
        }

        public SingleBuffer<SpriteUniform> Uniform
        {
            get => _uniform;
            set
            {
                if (_uniform == value)
                    return;
                _uniform = value;
                Dirty();
            }
        }

        protected override ResourceSet Build(GraphicsDevice device, ResourceLayout layout) =>
            device.ResourceFactory.CreateResourceSet(new ResourceSetDescription(
                layout,
                _texture.TextureView,
                _sampler.Sampler,
                _uniform.DeviceBuffer));
    }
}
