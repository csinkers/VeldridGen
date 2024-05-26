namespace VeldridGen.Example.Engine.Events;

public class BeginFrameEvent : IVerboseEvent
{
    public static BeginFrameEvent Instance { get; } = new();
}