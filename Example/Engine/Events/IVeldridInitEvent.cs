using Veldrid;

namespace VeldridGen.Example.Engine.Events;

public interface IVeldridInitEvent : IEvent
{
    GraphicsDevice Device { get; }
    CommandList CommandList { get; }
}