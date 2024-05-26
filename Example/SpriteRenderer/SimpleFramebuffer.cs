using Veldrid;
using VeldridGen.Example.Engine;
using VeldridGen.Interfaces;

namespace VeldridGen.Example.SpriteRenderer
{
    public partial class SimpleFramebuffer : FramebufferHolder
    {
        [DepthAttachment(PixelFormat.D24_UNorm_S8_UInt)] public Texture2DHolder Depth { get; }
        [ColorAttachment(PixelFormat.B8_G8_R8_A8_UNorm)] public Texture2DHolder Color { get; }
    }
}

