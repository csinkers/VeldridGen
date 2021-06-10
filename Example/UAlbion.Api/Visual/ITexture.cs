using System;
using System.Collections.Generic;

namespace UAlbion.Api.Visual
{
    public interface ITexture
    {
        IAssetId Id { get; }
        string Name { get; }
        int Width { get; }
        int Height { get; }
        int SizeInBytes { get; }
        IReadOnlyList<Region> Regions { get; }
        TextureDirtyType DirtyType { get; }
        int DirtyId { get; }
        void Clean();
    }

    public interface IArrayTexture : ITexture
    {
        int ArrayLayers { get; }
    }

    public interface IDepthTexture : ITexture
    {
        int Depth { get; }
    }

    public interface IReadOnlyTexture<T> : ITexture where T : unmanaged
    {
        ReadOnlySpan<T> PixelData { get; }
        ReadOnlyImageBuffer<T> GetRegionBuffer(int i);
    }

    public interface IMutableTexture<T> : IReadOnlyTexture<T> where T : unmanaged
    {
        Span<T> MutablePixelData { get; }
        ImageBuffer<T> GetMutableRegionBuffer(int i);
    }

    public interface IReadOnlyArrayTexture<T> : IReadOnlyTexture<T>, IArrayTexture where T : unmanaged
    {
        ReadOnlyImageBuffer<T> GetLayerBuffer(int i);
    }

    public interface IMutableArrayTexture<T> : IMutableTexture<T>, IReadOnlyArrayTexture<T> where T : unmanaged
    {
        ImageBuffer<T> GetMutableLayerBuffer(int i);
    }
}
