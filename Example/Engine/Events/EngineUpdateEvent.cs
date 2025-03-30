namespace VeldridGen.Example.Engine.Events;

class EngineUpdateEvent(float deltaSeconds) : IVerboseEvent
{
    public float DeltaSeconds { get; } = deltaSeconds;
}
