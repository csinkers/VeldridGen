using System;
using UAlbion.Api;
using Veldrid;

namespace UAlbion.Core.Veldrid.Events
{
    public class DeviceCreatedEvent : IEvent
    {
        public DeviceCreatedEvent(GraphicsDevice device) => Device = device ?? throw new ArgumentNullException(nameof(device));
        public GraphicsDevice Device { get; }
    }
}