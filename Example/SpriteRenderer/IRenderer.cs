using Veldrid;
using VeldridGen.Example.Engine;
using VeldridGen.Example.Engine.Visual;
using VeldridGen.Interfaces;

namespace VeldridGen.Example.SpriteRenderer
{
    public interface IRenderer : IComponent
    {
        void Render(IRenderable renderable, CommonSet commonSet, IFramebufferHolder framebuffer, CommandList cl, GraphicsDevice device);
    }
}
