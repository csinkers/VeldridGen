﻿using System;
using Veldrid;

namespace VeldridGen.Example.Engine.Events;

public class PrepareFrameResourceSetsEvent : IVeldridInitEvent
{
    public PrepareFrameResourceSetsEvent(GraphicsDevice device, CommandList commandList)
    {
        Device = device ?? throw new ArgumentNullException(nameof(device));
        CommandList = commandList ?? throw new ArgumentNullException(nameof(commandList));
    }

    public GraphicsDevice Device { get; }
    public CommandList CommandList { get; }
}
