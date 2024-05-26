using VeldridGen.Example.Engine.Visual;

namespace VeldridGen.Example.Engine;

public static class BlitUtil
{
    public static void BlitDirect<T>(ReadOnlyImageBuffer<T> fromBuffer, ImageBuffer<T> toBuffer) where T : unmanaged
    {
        for (int j = 0; j < fromBuffer.Height; j++)
        {
            var fromSlice = fromBuffer.Buffer.Slice(j * fromBuffer.Stride, fromBuffer.Width);
            var toSlice = toBuffer.Buffer.Slice(j * toBuffer.Stride, toBuffer.Width);
            fromSlice.CopyTo(toSlice);
        }
    }
}
