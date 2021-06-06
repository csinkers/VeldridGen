using System.ComponentModel;
using UAlbion.CodeGen;
using Veldrid;

namespace UAlbion.Core.SpriteBatch
{
    public partial class SpriteSet : Component, IResourceLayout
    {
        [Resource("uSprite", ShaderStages.Fragment)] Texture2DArrayHolder _texture;
        [Resource("uSpriteSampler", ShaderStages.Fragment)] SamplerHolder _sampler;
        [Resource("_Uniform")] SingleBuffer<SpriteUniform> _uniform;
    }

    // To be generated
    public partial class SpriteSet
    {
        public static readonly ResourceLayoutDescription Layout = new(
            new ResourceLayoutElementDescription("uSprite", ResourceKind.TextureReadOnly, ShaderStages.Fragment),
            new ResourceLayoutElementDescription("uSpriteSampler", ResourceKind.Sampler, ShaderStages.Fragment),
            new ResourceLayoutElementDescription("_Uniform", ResourceKind.UniformBuffer, ShaderStages.Vertex | ShaderStages.Fragment));

        ResourceSet _resourceSet;
        string _name;

        public SpriteSet()
        {
            On<DeviceCreatedEvent>(_ => Dirty());
            On<DestroyDeviceObjectsEvent>(_ => Dispose());
        }

        public ResourceSet DeviceSet => _resourceSet;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                if (_resourceSet != null)
                    _resourceSet.Name = _name;
            }
        }

        public Texture2DArrayHolder Texture
        {
            get => _texture;
            set
            {
                if (_texture == value)
                    return;

                if (_texture != null)
                    _texture.PropertyChanged -= DependencyPropertyChanged;

                _texture = value;
                
                if(_texture != null)
                    _texture.PropertyChanged += DependencyPropertyChanged;
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
                    _sampler.PropertyChanged -= DependencyPropertyChanged;

                _sampler = value;

                if (_sampler != null)
                    _sampler.PropertyChanged += DependencyPropertyChanged;
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

        protected override void Subscribed() => Dirty();
        protected override void Unsubscribed() => Dispose();
        void Dirty() => On<PrepareFrameResourceSetsEvent>(Update);
        void DependencyPropertyChanged(object sender, PropertyChangedEventArgs e) => Dirty();

        void Update(IVeldridInitEvent e)
        {
            if (_resourceSet != null)
                Dispose();

            // Ensure dependencies are up to date
            _texture.Receive(e, this);
            _sampler.Receive(e, this);
            _uniform.Receive(e, this);

            var layoutSource = Resolve<IResourceLayoutSource>();
            _resourceSet = e.Device.ResourceFactory.CreateResourceSet(new ResourceSetDescription(
                layoutSource.Get(GetType(), e.Device),
                _texture.TextureView,
                _sampler.Sampler,
                _uniform.DeviceBuffer));

            _resourceSet.Name = Name;
            Off<PrepareFrameResourcesEvent>();
        }

        public void Dispose()
        {
            _resourceSet?.Dispose();
            _resourceSet = null;
        }
    }
}