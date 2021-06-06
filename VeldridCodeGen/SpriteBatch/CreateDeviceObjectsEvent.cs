using System;
using UAlbion.Api;
using Veldrid;

namespace UAlbion.Core.SpriteBatch
{
    public class CreateDeviceObjectsEvent : Event, IVeldridInitEvent
    {
        public CreateDeviceObjectsEvent(GraphicsDevice device, CommandList commandList)
        {
            Device = device ?? throw new ArgumentNullException(nameof(device));
            CommandList = commandList ?? throw new ArgumentNullException(nameof(commandList));
        }

        public GraphicsDevice Device { get; }
        public CommandList CommandList { get; }
    }
}