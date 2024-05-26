namespace VeldridGen.Example.Engine.Events;

class WindowResizedEvent : IEvent
{
    public int Width { get; }
    public int Height { get; }
    public WindowResizedEvent(int width, int height) { Width = width; Height = height; }
}
