using System.ComponentModel;
using UAlbion.CodeGen;
using UAlbion.Core.SpriteBatch;
using Veldrid;

namespace UAlbion.Core
{
    public partial class CommonSet : Component, IResourceLayout
    {
        [Resource("_Shared")]     SingleBuffer<GlobalInfo> _globalInfo;
        [Resource("_Projection")] SingleBuffer<ProjectionMatrix> _projection;
        [Resource("_View")]       SingleBuffer<ViewMatrix> _view;
        [Resource("uPalette")]    Texture2DHolder _palette;
    }

    // To be generated
    public partial class CommonSet
    {
        public static readonly ResourceLayoutDescription Layout = new(
            new ResourceLayoutElementDescription("_Shared", ResourceKind.UniformBuffer, ShaderStages.Fragment | ShaderStages.Vertex),
            new ResourceLayoutElementDescription("_Projection", ResourceKind.UniformBuffer, ShaderStages.Vertex),
            new ResourceLayoutElementDescription("_View", ResourceKind.UniformBuffer, ShaderStages.Vertex),
            new ResourceLayoutElementDescription("uPalette", ResourceKind.TextureReadOnly, ShaderStages.Fragment));

        ResourceSet _resourceSet;
        string _name;

        public CommonSet()
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

        public SingleBuffer<GlobalInfo> GlobalInfo
        {
            get => _globalInfo;
            set
            {
                if (_globalInfo == value)
                    return;
                _globalInfo = value;
                Dirty();
            }
        }

        public SingleBuffer<ProjectionMatrix> Projection
        {
            get => _projection;
            set
            {
                if (_projection == value) return;
                _projection = value;
                Dirty();
            }
        }

        public SingleBuffer<ViewMatrix> View
        {
            get => _view;
            set
            {
                if (_view == value) return;
                _view = value;
                Dirty();
            }
        }

        public Texture2DHolder Palette
        {
            get => _palette;
            set
            {
                if (_palette == value) return;

                if (_palette != null)
                    _palette.PropertyChanged -= InvalidatePalette;

                _palette = value;

                if (_palette != null)
                    _palette.PropertyChanged += InvalidatePalette;
                Dirty();
            }
        }

        void InvalidatePalette(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(Palette.TextureView))
                return;
            Dirty();
        }

        protected override void Subscribed() => Dirty();
        protected override void Unsubscribed() => Dispose();
        void Dirty() => On<PrepareFrameResourcesEvent>(Update);

        void Update(IVeldridInitEvent e)
        {
            if (_resourceSet != null)
                Dispose();

            _globalInfo.Receive(e, this);

            var layoutSource = Resolve<IResourceLayoutSource>();
            _resourceSet = e.Device.ResourceFactory.CreateResourceSet(new ResourceSetDescription(
                layoutSource.Get(GetType(), e.Device),
                _globalInfo.DeviceBuffer,
                _projection.DeviceBuffer,
                _view.DeviceBuffer,
                _palette.TextureView));

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
