using System;
using Veldrid;

namespace VeldridGen.Example.Engine.Events;

public class PrepareFrameResourcesEvent(GraphicsDevice device, CommandList commandList) : IVeldridInitEvent
{
    public GraphicsDevice Device { get; } = device ?? throw new ArgumentNullException(nameof(device));
    public CommandList CommandList { get; } = commandList ?? throw new ArgumentNullException(nameof(commandList));
}
