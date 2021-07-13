using Veldrid;
using VeldridGen.Interfaces;

namespace VeldridGen.Example.Engine
{
    public interface ISceneRenderer
    {
        void Render(GraphicsDevice device, CommandList cl);
        IFramebufferHolder Framebuffer { get; }
    }
}