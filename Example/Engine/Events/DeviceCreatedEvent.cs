using System;
using Veldrid;

namespace VeldridGen.Example.Engine.Events;

public class DeviceCreatedEvent(GraphicsDevice device) : IEvent
{
    public GraphicsDevice Device { get; } = device ?? throw new ArgumentNullException(nameof(device));
}
