namespace VeldridGen.Example.Engine.Events;

class WindowResizedEvent(int width, int height) : IEvent
{
    public int Width { get; } = width;
    public int Height { get; } = height;
}
