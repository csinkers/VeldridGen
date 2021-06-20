﻿using UAlbion.Api;

namespace UAlbion.Core.Events
{
    [Event("e:begin_frame", "Emitted at the beginning of each frame to allow components to clear any per-frame state.")]
    public class BeginFrameEvent : IVerboseEvent
    {
        public static BeginFrameEvent Instance { get; } = new();
    }
}
