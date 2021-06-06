using UAlbion.Api;

namespace UAlbion.Core
{
    class EngineUpdateEvent : Event, IVerboseEvent
    {
        public double DeltaSeconds { get; }
        public EngineUpdateEvent(double deltaSeconds) => DeltaSeconds = deltaSeconds;
    }
}