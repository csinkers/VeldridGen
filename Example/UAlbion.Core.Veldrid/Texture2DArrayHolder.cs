using System;
using System.ComponentModel;
using UAlbion.Api.Visual;
using UAlbion.Core.Veldrid.Events;
using Veldrid;
using VeldridGen.Interfaces;

namespace UAlbion.Core.Veldrid
{
    public class Texture2DArrayHolder : Component, ITextureArrayHolder
    {
        IArrayTexture _texture;
        Texture _deviceTexture;
        TextureView _textureView;

        public string Glsl(int set, int binding, string name)
            => $@"layout(set = {set}, binding = {binding}) uniform texture2DArray {name}; //! // vdspv_{set}_{binding}";

        public IArrayTexture Texture
        {
            get => _texture;
            set { _texture = value; Dirty();}
        }

        public Texture DeviceTexture
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

        public event PropertyChangedEventHandler PropertyChanged;

        public Texture2DArrayHolder(IArrayTexture texture)
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
                IReadOnlyArrayTexture<byte> eightBitArray => VeldridTexture.CreateArrayTexture(e.Device, TextureUsage.Sampled, eightBitArray),
                IReadOnlyArrayTexture<uint> trueColorArray => VeldridTexture.CreateArrayTexture(e.Device, TextureUsage.Sampled | TextureUsage.GenerateMipmaps, trueColorArray),
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
    }
}
