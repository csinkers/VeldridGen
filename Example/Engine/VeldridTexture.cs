using System;
using Veldrid;
using VeldridGen.Example.Engine.Visual;

namespace VeldridGen.Example.Engine;

public static class VeldridTexture
{
    static uint MipLevelCount(int width, int height)
    {
        int maxDimension = Math.Max(width, height);
        int levels = 1;
        while (maxDimension > 1)
        {
            maxDimension >>= 1;
            levels++;
        }
        return (uint)levels;
    }

    static PixelFormat GetFormat(Type pixelType)
    {
        if (pixelType == typeof(byte)) return PixelFormat.R8_UNorm;
        if (pixelType == typeof(uint)) return PixelFormat.R8_G8_B8_A8_UNorm;
        throw new NotSupportedException();
    }

    public static unsafe Texture Create<T>(GraphicsDevice gd, TextureUsage usage, IReadOnlyTexture<T> texture) where T : unmanaged
    {
        if (gd == null) throw new ArgumentNullException(nameof(gd));
        if (texture == null) throw new ArgumentNullException(nameof(texture));

        var pixelFormat = GetFormat(typeof(T));
        bool mip = (usage & TextureUsage.GenerateMipmaps) != 0;
        uint mipLevels = mip ? MipLevelCount(texture.Width, texture.Height) : 1;
        using Texture staging = gd.ResourceFactory.CreateTexture(new TextureDescription(
            (uint)texture.Width, (uint)texture.Height, 1, mipLevels,
            (uint)texture.ArrayLayers,
            pixelFormat,
            TextureUsage.Staging,
            TextureType.Texture2D));

        staging.Name = "T_" + texture.Name + "_Staging";

        for (int layer = 0; layer < texture.ArrayLayers; layer++)
        {
            var mapped = gd.Map(staging, MapMode.Write, (uint)layer * mipLevels);
            try
            {
                var span = new Span<T>(mapped.Data.ToPointer(), (int)mapped.SizeInBytes / sizeof(T));
                int pitch = (int)(mapped.RowPitch / sizeof(T));

                var source = texture.GetLayerBuffer(layer);
                var dest = new ImageBuffer<T>(texture.Width, texture.Height, pitch, span);

                BlitUtil.BlitDirect(source, dest);

                //gd.UpdateTexture(
                //    staging, (IntPtr) texDataPtr, (uint) (buffer.Buffer.Length * Unsafe.SizeOf<T>()),
                //    0, 0, 0,
                //    (uint) texture.Width, (uint) texture.Height, 1,
                //    0, (uint) layer);
            }
            finally { gd.Unmap(staging, (uint)layer * mipLevels); }
        }

        Texture veldridTexture = gd.ResourceFactory.CreateTexture(new TextureDescription(
            (uint)texture.Width, (uint)texture.Height, 1,
            mipLevels,
            (uint)texture.ArrayLayers,
            pixelFormat,
            usage,
            TextureType.Texture2D));

        veldridTexture.Name = "T_" + texture.Name;

        using CommandList cl = gd.ResourceFactory.CreateCommandList();
        cl.Begin();
        cl.CopyTexture(staging, veldridTexture);
        if (mip) cl.GenerateMipmaps(veldridTexture);
        cl.End();
        gd.SubmitCommands(cl);

        return veldridTexture;
    }
}