using UAlbion.Api;
using Veldrid;

namespace UAlbion.Core.SpriteBatch
{
    public interface IVeldridInitEvent : IEvent
    {
        GraphicsDevice Device { get; }
        CommandList CommandList { get; }
    }
}