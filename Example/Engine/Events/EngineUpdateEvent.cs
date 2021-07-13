namespace VeldridGen.Example.Engine.Events
{
    class EngineUpdateEvent : IVerboseEvent
    {
        public double DeltaSeconds { get; }
        public EngineUpdateEvent(double deltaSeconds) => DeltaSeconds = deltaSeconds;
    }
}