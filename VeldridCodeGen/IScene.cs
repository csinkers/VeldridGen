using Veldrid;

namespace UAlbion.Core
{
    public interface IScene
    {
        void Render(GraphicsDevice device, CommandList cl);
    }
}