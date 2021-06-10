using UAlbion.Api;

namespace UAlbion.Core.Veldrid.Events
{
    class WindowResizedEvent : Event
    {
        public int Width { get; }
        public int Height { get; }
        public WindowResizedEvent(int width, int height) { Width = width; Height = height; }
    }
}