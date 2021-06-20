using System.Linq;
using System.Text;

namespace VeldridGen
{
    static class FramebufferGenerator
    {
        public static void Generate(StringBuilder sb, VeldridTypeInfo type)
        {
            // TODO: Decouple from UAlbion.Core etc, make more flexible
            var depth = type.Members.SingleOrDefault(x => x.DepthAttachment != null);
            sb.AppendLine(@"        protected override Framebuffer CreateFramebuffer(global::UAlbion.Core.Veldrid.Events.IVeldridInitEvent e)
        {");

            if (depth != null)
            {
                sb.AppendLine($@"            {depth.Symbol.Name} = e.Device.ResourceFactory.CreateTexture(new TextureDescription(
                    Width, Height, 1, 1, 1,
                    global::{depth.DepthAttachment.Format}, TextureUsage.DepthStencil, TextureType.Texture2D));
");
            }

            foreach (var color in type.Members.Where(member => member.ColorAttachment != null))
            {
                sb.AppendLine($@"            {color.Symbol.Name} = e.Device.ResourceFactory.CreateTexture(new TextureDescription(
                    Width, Height, 1, 1, 1,
                    global::{color.ColorAttachment.Format}, TextureUsage.RenderTarget, TextureType.Texture2D));
");
            }

            sb.Append("            var description = new FramebufferDescription(");
            sb.Append(depth != null ? depth.Symbol.Name : "null");

            foreach (var member in type.Members.Where(member => member.ColorAttachment != null))
            {
                sb.Append(", ");
                sb.Append(member.Symbol.Name);
            }

            sb.AppendLine(@");
            return e.Device.ResourceFactory.CreateFramebuffer(ref description);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);");
            if (depth != null)
            {
                sb.AppendLine($@"            {depth.Symbol.Name}?.Dispose();");
                sb.AppendLine($@"            {depth.Symbol.Name} = null;");
            }

            foreach (var member in type.Members.Where(member => member.ColorAttachment != null))
            {
                sb.AppendLine($@"            {member.Symbol.Name}?.Dispose();");
                sb.AppendLine($@"            {member.Symbol.Name} = null;");
            }

            sb.AppendLine(@"        }");
        }
        /* e.g.
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
        } */
    }
}
