﻿using Veldrid;
namespace UAlbion.Core.SpriteRenderer
{
    public partial class CommonSet
    {
        public static readonly ResourceLayoutDescription Layout = new(
            new ResourceLayoutElementDescription("_Shared", global::Veldrid.ResourceKind.UniformBuffer, (ShaderStages)17),
            new ResourceLayoutElementDescription("_Projection", global::Veldrid.ResourceKind.UniformBuffer, (ShaderStages)1),
            new ResourceLayoutElementDescription("_View", global::Veldrid.ResourceKind.UniformBuffer, (ShaderStages)1),
            new ResourceLayoutElementDescription("uPalette", global::Veldrid.ResourceKind.TextureReadOnly, (ShaderStages)16));

        public global::UAlbion.Core.Veldrid.SingleBuffer<global::UAlbion.Core.SpriteRenderer.GlobalInfo> GlobalInfo
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

        public global::UAlbion.Core.Veldrid.SingleBuffer<global::UAlbion.Core.SpriteRenderer.ProjectionMatrix> Projection
        {
            get => _projection;
            set
            {
                if (_projection == value)
                    return;
                _projection = value;
                Dirty();
            }
        }

        public global::UAlbion.Core.Veldrid.SingleBuffer<global::UAlbion.Core.SpriteRenderer.ViewMatrix> View
        {
            get => _view;
            set
            {
                if (_view == value)
                    return;
                _view = value;
                Dirty();
            }
        }

        public global::UAlbion.Core.Veldrid.Texture2DHolder Palette
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
                _palette.DeviceTexture));
    }
}
