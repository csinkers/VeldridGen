using System;

namespace VeldridGen.Example.Engine.Visual;

public delegate ReadOnlyImageBuffer<T> GetFrameDelegate<T>(int frame);

[System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Never compared")]
public readonly ref struct ReadOnlyImageBuffer<T>(int width, int height, int stride, ReadOnlySpan<T> buffer)
{
    public ReadOnlyImageBuffer(ReadOnlyImageBuffer<T> existing, ReadOnlySpan<T> buffer) : this(existing.Width, existing.Height, existing.Stride, buffer)
    {
    }

    public int Width { get; } = width;
    public int Height { get; } = height;
    public int Stride { get; } = stride;
    public ReadOnlySpan<T> Buffer { get; } = buffer;
    public ReadOnlySpan<T> GetRow(int row) => Buffer.Slice(Stride * row, Width);
}
