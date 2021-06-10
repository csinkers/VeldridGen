using UAlbion.Core.Veldrid;
using Veldrid;

namespace UAlbion.Core.SpriteRenderer
{
    // To be generated
    public partial class CommonSet
    {
        public static readonly ResourceLayoutDescription Layout = new(
            new ResourceLayoutElementDescription("_Shared", ResourceKind.UniformBuffer, ShaderStages.Fragment | ShaderStages.Vertex),
            new ResourceLayoutElementDescription("_Projection", ResourceKind.UniformBuffer, ShaderStages.Vertex),
            new ResourceLayoutElementDescription("_View", ResourceKind.UniformBuffer, ShaderStages.Vertex),
            new ResourceLayoutElementDescription("uPalette", ResourceKind.TextureReadOnly, ShaderStages.Fragment));

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
                    _palette.PropertyChanged -= PropertyDirty;

                _palette = value;

                if (_palette != null)
                    _palette.PropertyChanged += PropertyDirty;
                Dirty();
            }
        }

        protected override ResourceSet Build(GraphicsDevice device, ResourceLayout layout) =>
            device.ResourceFactory.CreateResourceSet(new ResourceSetDescription(
                layout,
                _globalInfo.DeviceBuffer,
                _projection.DeviceBuffer,
                _view.DeviceBuffer,
                _palette.TextureView));
    }
}
