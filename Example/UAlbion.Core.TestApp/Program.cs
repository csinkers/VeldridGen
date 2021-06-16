using System;
using System.IO;
using System.Numerics;
using System.Reflection;
using UAlbion.Api;
using UAlbion.Api.Visual;
using UAlbion.Core.SpriteRenderer;
using UAlbion.Core.Sprites;
using UAlbion.Core.Veldrid;
using Veldrid;
using VeldridGen.Interfaces;

namespace UAlbion.Core.TestApp
{
    static class Program
    {
        static string FindRoot(string binDir)
        {
            var curDir = new DirectoryInfo(binDir);

            while (curDir != null && !Directory.Exists(Path.Combine(curDir.FullName, "UAlbion.Core.Veldrid")))
                curDir = curDir.Parent;

            return curDir?.FullName;
        }

        static void Main()
        {
            var binDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var rootDir = FindRoot(binDir);
            if (rootDir == null)
                throw new InvalidOperationException("Could not find root directory");

            var exchange = new EventExchange();
            var fileSystem = new FileSystem();
            var shaderCache = new ShaderCache(Path.Combine(binDir, "Shaders"));
            var layoutSource = new ResourceLayoutSource();
            var samplerSource = new SpriteSamplerSource();
            var textureSource = new TextureSource();
            var framebuffer = new MainFramebuffer();
            var palette = BuildPalette();
            var texture = BuildTexture();
            var scene = new SpriteScene(palette, framebuffer);
            var engine = new Engine(GraphicsBackend.Direct3D11, true, scene);

            shaderCache.AddShaderPath(Path.Combine(rootDir, @"UAlbion.Core.SpriteRenderer\Shaders"));

            exchange
                .Register<IFileSystem>(fileSystem)
                .Register<IResourceLayoutSource>(layoutSource)
                .Register<ISpriteSamplerSource>(samplerSource)
                .Register<ITextureSource>(textureSource)
                .Register<IShaderCache>(shaderCache)
                .Attach(framebuffer)
                .Attach(scene)
                .Attach(engine)
                ;

            var key = new SpriteKey(texture, SpriteSampler.Default, SpriteKeyFlags.UsePalette);
            var width = 200;
            var height = 100;
            var lease = scene.SpriteManager.Borrow(key, 5 * width * height, null);
            for (int j = 0; j < height; j++)
                for (int i = 0; i < width; i++)
                    Set(i, j, width, height, lease, texture);
            engine.Run();
        }

        static void Set(int x, int y, int width, int height, ISpriteLease lease, ITexture texture)
        {
            int baseIndex = (y * width + x) * 5;
            var center = new Vector3(
                16.0f*((float)x/width - 0.5f),
                16.0f*((float)y/height - 0.5f),
                -10);

            var size = new Vector2((float)x / (8*width), (float)y / (8*height));
            var size3 = new Vector3(size.X, size.Y, 0);

            lease.Set(baseIndex + 0, center + size3 * new Vector3(0, 0, 0),   size, texture.Regions[0], SpriteFlags.None);
            lease.Set(baseIndex + 1, center + size3 * new Vector3(-1, -1, 0), size, texture.Regions[1], SpriteFlags.None);
            lease.Set(baseIndex + 2, center + size3 * new Vector3(-1, 1, 0),  size, texture.Regions[2], SpriteFlags.None);
            lease.Set(baseIndex + 3, center + size3 * new Vector3(1, -1, 0),  size, texture.Regions[3], SpriteFlags.None);
            lease.Set(baseIndex + 4, center + size3 * new Vector3(1, 1, 0),   size, texture.Regions[4], SpriteFlags.None);
        }

        static SimpleTexture<uint> BuildPalette()
        {
            var palette = new SimpleTexture<uint>(new AssetId(0, "Palette"), 256, 1);
            var pixels = palette.MutablePixelData;
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = ApiUtil.PackColor((byte)i, (byte)i, (byte)i, 0xff);
            pixels[0] = 0; // 0 is transparent
            return palette;
        }

        static ArrayTexture<byte> BuildTexture()
        {
            var texture = new ArrayTexture<byte>(new AssetId(1, "Test Texture"), 64, 64, 5);
            {
                texture.AddRegion(0, 0, 64, 64);
                var pixels = texture.GetMutableRegionBuffer(0);
                for (int j = 0; j < pixels.Height; j++)
                    for (int i = 0; i < pixels.Width; i++)
                        pixels.Buffer[j * pixels.Width + i] = i == 0 || j == 0 ? (byte)255 : (byte)(2 * (i + j));
            }

            {
                texture.AddRegion(0, 0, 64, 64, 1);
                var pixels = texture.GetMutableRegionBuffer(1);
                for (int j = 0; j < pixels.Height; j++)
                    for (int i = 0; i < pixels.Width; i++)
                        pixels.Buffer[j * pixels.Width + i] = i == 0 || j == 0 ? (byte)255 : (byte)(255 - 2 * (i + j));
            }

            {

                texture.AddRegion(0, 0, 64, 64, 2);
                var pixels = texture.GetMutableRegionBuffer(2);
                for (int j = 0; j < pixels.Height; j++)
                    for (int i = 0; i < pixels.Width; i++)
                        pixels.Buffer[j * pixels.Width + i] = i == 0 || j == 0 ? (byte)255 : (byte)((i - j));
            }

            {

                texture.AddRegion(0, 0, 64, 64, 3);
                var pixels = texture.GetMutableRegionBuffer(3);
                for (int j = 0; j < pixels.Height; j++)
                    for (int i = 0; i < pixels.Width; i++)
                        pixels.Buffer[j * pixels.Width + i] = i == 0 || j == 0 ? (byte)255 : (byte)(i * j);
            }

            {

                texture.AddRegion(0, 0, 64, 64, 4);
                var pixels = texture.GetMutableRegionBuffer(4);
                for (int j = 0; j < pixels.Height; j++)
                    for (int i = 0; i < pixels.Width; i++)
                        pixels.Buffer[j * pixels.Width + i] = i == 0 || j == 0 ? (byte)255 : (byte)(i % j);
            }
            return texture;
        }
    }
}
