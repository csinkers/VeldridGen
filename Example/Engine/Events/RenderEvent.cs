using System;
using Veldrid;

namespace VeldridGen.Example.Engine.Events;

public class RenderEvent(GraphicsDevice graphicsDevice, CommandList frameCommands) : IVerboseEvent
{
    public GraphicsDevice GraphicsDevice { get; } = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
    public CommandList FrameCommands { get; } = frameCommands ?? throw new ArgumentNullException(nameof(frameCommands));
}
