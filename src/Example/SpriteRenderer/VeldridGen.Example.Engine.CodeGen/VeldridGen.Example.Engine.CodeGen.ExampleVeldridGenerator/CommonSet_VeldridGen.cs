﻿using Veldrid;
namespace VeldridGen.Example.SpriteRenderer
{
    public partial class CommonSet
    {
        public static readonly ResourceLayoutDescription Layout = new(
            new ResourceLayoutElementDescription("_Shared", global::Veldrid.ResourceKind.UniformBuffer, (ShaderStages)17),
            new ResourceLayoutElementDescription("_Projection", global::Veldrid.ResourceKind.UniformBuffer, (ShaderStages)1),
            new ResourceLayoutElementDescription("_View", global::Veldrid.ResourceKind.UniformBuffer, (ShaderStages)1),
            new ResourceLayoutElementDescription("uPalette", global::Veldrid.ResourceKind.TextureReadOnly, (ShaderStages)16));

        public global::VeldridGen.Example.Engine.SingleBuffer<global::VeldridGen.Example.SpriteRenderer.GlobalInfo> GlobalInfo
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

        public global::VeldridGen.Example.Engine.SingleBuffer<global::VeldridGen.Example.SpriteRenderer.ProjectionMatrix> Projection
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

        public global::VeldridGen.Example.Engine.SingleBuffer<global::VeldridGen.Example.SpriteRenderer.ViewMatrix> View
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

        public global::VeldridGen.Interfaces.ITextureHolder Palette
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

        protected override ResourceSet Build(GraphicsDevice device, ResourceLayout layout)
        {
#if DEBUG
                if (_globalInfo.DeviceBuffer == null) throw new System.InvalidOperationException("Tried to construct CommonSet, but GlobalInfo has not been initialised. It may not have been attached to the exchange.");
                if (_projection.DeviceBuffer == null) throw new System.InvalidOperationException("Tried to construct CommonSet, but Projection has not been initialised. It may not have been attached to the exchange.");
                if (_view.DeviceBuffer == null) throw new System.InvalidOperationException("Tried to construct CommonSet, but View has not been initialised. It may not have been attached to the exchange.");
                if (_palette.DeviceTexture == null) throw new System.InvalidOperationException("Tried to construct CommonSet, but Palette has not been initialised. It may not have been attached to the exchange.");
#endif

            return device.ResourceFactory.CreateResourceSet(new ResourceSetDescription(
                layout,
                _globalInfo.DeviceBuffer,
                _projection.DeviceBuffer,
                _view.DeviceBuffer,
                _palette.DeviceTexture));
        }

        protected override void Resubscribe()
        {
            if (_palette != null)
                _palette.PropertyChanged += PropertyDirty;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (_palette != null)
                _palette.PropertyChanged -= PropertyDirty;
        }
    }
}
