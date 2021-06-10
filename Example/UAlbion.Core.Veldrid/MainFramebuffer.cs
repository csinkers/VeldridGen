using UAlbion.Core.Veldrid.Events;
using Veldrid;

namespace UAlbion.Core.Veldrid
{
    public class MainFramebuffer : FramebufferHolder
    {
        public MainFramebuffer() : base(0, 0) { }
        protected override Framebuffer CreateFramebuffer(IVeldridInitEvent e) 
            => e.Device.SwapchainFramebuffer;
    }
}