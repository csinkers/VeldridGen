using System;

namespace VeldridGen.Example.Engine.Visual;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Won't be compared")]
public readonly ref struct ImageBuffer<T>(int width, int height, int stride, Span<T> buffer)
    where T : unmanaged
{
    public ImageBuffer(ImageBuffer<T> existing, Span<T> buffer) : this(existing.Width, existing.Height, existing.Stride, buffer)
    {
    }

    public int Width { get; } = width;
    public int Height { get; } = height;
    public int Stride { get; } = stride;
    public Span<T> Buffer { get; } = buffer;
    public Span<T> GetRow(int row) => Buffer.Slice(Stride * row, Width);
}
