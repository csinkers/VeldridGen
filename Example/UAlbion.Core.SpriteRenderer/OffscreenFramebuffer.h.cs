using UAlbion.Core.Veldrid.Events;
using Veldrid;

namespace UAlbion.Core.SpriteRenderer
{
    // To be generated
    public partial class OffscreenFramebuffer
    {
        protected override Framebuffer CreateFramebuffer(IVeldridInitEvent e)
        {
            _depth = e.Device.ResourceFactory.CreateTexture(new TextureDescription(
                Width, Height, 1, 1, 1,
                PixelFormat.R32_Float, TextureUsage.DepthStencil, TextureType.Texture2D));

            _color = e.Device.ResourceFactory.CreateTexture(new TextureDescription(
                Width, Height, 1, 1, 1,
                PixelFormat.B8_G8_R8_A8_UNorm, TextureUsage.RenderTarget, TextureType.Texture2D));

            var description = new FramebufferDescription(_depth, _color);
            return e.Device.ResourceFactory.CreateFramebuffer(ref description);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _depth?.Dispose();
            _color?.Dispose();
            _depth = null;
            _color = null;
        }
    }
}
