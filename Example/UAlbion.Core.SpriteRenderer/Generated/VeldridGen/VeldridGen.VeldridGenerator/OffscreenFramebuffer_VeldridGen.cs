using Veldrid;
namespace UAlbion.Core.SpriteRenderer
{
    public partial class OffscreenFramebuffer
    {
        protected override Framebuffer CreateFramebuffer(global::Veldrid.GraphicsDevice device)
        {
            _depth = device.ResourceFactory.CreateTexture(new TextureDescription(
                    Width, Height, 1, 1, 1,
                    global::Veldrid.PixelFormat.R32_Float, TextureUsage.DepthStencil, TextureType.Texture2D));

            _color = device.ResourceFactory.CreateTexture(new TextureDescription(
                    Width, Height, 1, 1, 1,
                    global::Veldrid.PixelFormat.B8_G8_R8_A8_UNorm, TextureUsage.RenderTarget, TextureType.Texture2D));

            var description = new FramebufferDescription(_depth, _color);
            return device.ResourceFactory.CreateFramebuffer(ref description);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _depth?.Dispose();
            _depth = null;
            _color?.Dispose();
            _color = null;
        }
    }
}
