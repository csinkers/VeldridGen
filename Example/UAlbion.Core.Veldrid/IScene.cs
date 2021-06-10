using Veldrid;

namespace UAlbion.Core.Veldrid
{
    public interface IScene
    {
        void Render(GraphicsDevice device, CommandList cl);
    }
}