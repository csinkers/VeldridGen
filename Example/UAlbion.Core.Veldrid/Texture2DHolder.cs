using System;
using System.ComponentModel;
using UAlbion.Api.Visual;
using UAlbion.Core.Veldrid.Events;
using Veldrid;
using VeldridCodeGen.Interfaces;

namespace UAlbion.Core.Veldrid
{
    public class Texture2DHolder : Component, ITextureHolder
    {
        public string Glsl(int set, int binding, string name)
            => $@"layout(set = {set}, binding = {binding}) uniform texture2D {name}; //! // vdspv_{set}_{binding}";

        ITexture _texture;
        Texture _deviceTexture;
        TextureView _textureView;

        public ITexture Texture
        {
            get => _texture;
            set { _texture = value; Dirty();}
        }

        public global::Veldrid.Texture DeviceTexture
        {
            get => _deviceTexture;
            private set
            {
                if (_deviceTexture == value)
                    return;
                _deviceTexture = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeviceTexture)));
            }
        }

        public TextureView TextureView
        {
            get => _textureView;
            private set
            {
                if (_textureView == value)
                    return;
                _textureView = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TextureView)));
            }
        }

        public Texture2DHolder(ITexture texture)
        {
            _texture = texture ?? throw new ArgumentNullException(nameof(texture));
            On<DestroyDeviceObjectsEvent>(_ => Dispose());
        }

        protected override void Subscribed() => Dirty();
        protected override void Unsubscribed() => Dispose();
        void Dirty() => On<PrepareFrameResourcesEvent>(Update);

        void Update(PrepareFrameResourcesEvent e)
        {
            Dispose();
            DeviceTexture = Texture switch
            { // Note: No Mip-mapping for 8-bit, blending/interpolation in palette-based images typically results in nonsense.
                IReadOnlyTexture<byte> eightBit => VeldridTexture.CreateSimpleTexture(e.Device, TextureUsage.Sampled, eightBit),
                IReadOnlyTexture<uint> trueColor => VeldridTexture.CreateSimpleTexture(e.Device, TextureUsage.Sampled | TextureUsage.GenerateMipmaps, trueColor),
                _ => throw new NotSupportedException($"Image format {Texture.GetType().GetGenericArguments()[0].Name} not currently supported")
            };

            var textureView = e.Device.ResourceFactory.CreateTextureView(DeviceTexture);
            textureView.Name = "TV_" + Texture.Name;
            TextureView = textureView;

            Off<PrepareFrameResourcesEvent>();
        }

        public void Dispose()
        {
            TextureView?.Dispose();
            DeviceTexture?.Dispose();
            TextureView = null;
            DeviceTexture = null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}