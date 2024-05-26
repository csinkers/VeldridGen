using System;
using Veldrid;

namespace VeldridGen.Example.Engine.Events;

public class DeviceCreatedEvent : IEvent
{
    public DeviceCreatedEvent(GraphicsDevice device) => Device = device ?? throw new ArgumentNullException(nameof(device));
    public GraphicsDevice Device { get; }
}
