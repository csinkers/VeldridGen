using System;
using Veldrid;

namespace VeldridGen.Example.Engine.Events
{
    public class RenderEvent : IVerboseEvent
    {
        public GraphicsDevice GraphicsDevice { get; }
        public CommandList FrameCommands { get; }

        public RenderEvent(GraphicsDevice graphicsDevice, CommandList frameCommands)
        {
            GraphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
            FrameCommands = frameCommands ?? throw new ArgumentNullException(nameof(frameCommands));
        }
    }
}
