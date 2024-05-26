namespace VeldridGen.Example.Engine.Events;

class EngineUpdateEvent : IVerboseEvent
{
    public float DeltaSeconds { get; }
    public EngineUpdateEvent(float deltaSeconds) => DeltaSeconds = deltaSeconds;
}
