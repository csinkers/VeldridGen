using UAlbion.Api;

namespace UAlbion.Core.Veldrid.Events
{
    class EngineUpdateEvent : IVerboseEvent
    {
        public double DeltaSeconds { get; }
        public EngineUpdateEvent(double deltaSeconds) => DeltaSeconds = deltaSeconds;
    }
}