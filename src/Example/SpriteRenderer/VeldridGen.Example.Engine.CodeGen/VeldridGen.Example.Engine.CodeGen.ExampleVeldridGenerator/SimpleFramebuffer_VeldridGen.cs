using Veldrid;
namespace VeldridGen.Example.SpriteRenderer
{
    public partial class SimpleFramebuffer
    {
        public SimpleFramebuffer(string name, uint width, uint height) : base(name, width, height)
        {
            Depth = new global::VeldridGen.Example.Engine.Texture2DHolder(name + ".Depth");
            Color = new global::VeldridGen.Example.Engine.Texture2DHolder(name + ".Color");
        }

        protected override Framebuffer CreateFramebuffer(global::Veldrid.GraphicsDevice device)
        {
            if (device == null) throw new System.ArgumentNullException(nameof(device));
            Depth.DeviceTexture = device.ResourceFactory.CreateTexture(new TextureDescription(
                    Width, Height, 1, 1, 1,
                    global::Veldrid.PixelFormat.D24_UNorm_S8_UInt, TextureUsage.DepthStencil, TextureType.Texture2D));
            Depth.DeviceTexture.Name = Depth.Name;

            Color.DeviceTexture = device.ResourceFactory.CreateTexture(new TextureDescription(
                    Width, Height, 1, 1, 1,
                    global::Veldrid.PixelFormat.B8_G8_R8_A8_UNorm, TextureUsage.RenderTarget | TextureUsage.Sampled, TextureType.Texture2D));
            Color.DeviceTexture.Name = Color.Name;

            var description = new FramebufferDescription(Depth.DeviceTexture, Color.DeviceTexture);
            var framebuffer = device.ResourceFactory.CreateFramebuffer(ref description);
            framebuffer.Name = Name;
            return framebuffer;
        }

        public static OutputDescription Output
        {
            get
            {
                OutputAttachmentDescription? depthAttachment = new(global::Veldrid.PixelFormat.D24_UNorm_S8_UInt);
                OutputAttachmentDescription[] colorAttachments =
                {
                    new(global::Veldrid.PixelFormat.B8_G8_R8_A8_UNorm)
                };
                return new OutputDescription(depthAttachment, colorAttachments);
            }
        }

        public override OutputDescription? OutputDescription => Output;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            Depth.DeviceTexture?.Dispose();
            Depth.DeviceTexture = null;
            Color.DeviceTexture?.Dispose();
            Color.DeviceTexture = null;
        }
    }
}
