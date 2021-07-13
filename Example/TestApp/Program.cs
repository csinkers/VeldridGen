using System;
using System.IO;
using System.Numerics;
using System.Reflection;
using Veldrid;
using VeldridGen.Example.Engine;
using VeldridGen.Example.Engine.Visual;
using VeldridGen.Example.Engine.Visual.Sprites;
using VeldridGen.Example.SpriteRenderer;

namespace VeldridGen.Example.TestApp
{
    static class Program
    {
        static string FindRoot(string binDir)
        {
            var curDir = new DirectoryInfo(binDir);

            while (curDir != null && !Directory.Exists(Path.Combine(curDir.FullName, "Engine")))
                curDir = curDir.Parent;

            return curDir?.FullName;
        }

        static int Main(string[] args)
        {
            if (args.Length == 1)
                return ShaderHeaderEmitter.EmitAll(args[0]);

            if (args.Length != 0)
            {
                Console.WriteLine("Unexpected arguments encountered. Valid arguments are: 0 to run normally, 1 argument (shader path) to emit shader headers");
                return 1;
            }

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
            var spriteManager = new SpriteManager();
            var spriteFactory = new SpriteFactory();
            var palette = BuildPalette();
            var paletteManager = new PaletteManager(palette);
            var texture = BuildTexture();
            var scene = new SceneRenderer("MainScene", framebuffer)
                .AddRenderer(new SpriteRenderer.SpriteRenderer(framebuffer), typeof(VeldridSpriteBatch))
                .AddSource(spriteManager)
                ;
            var camera = new PerspectiveCamera();
            var engine = new VeldridEngine(GraphicsBackend.Direct3D11, false, scene);

            shaderCache.AddShaderPath(Path.Combine(rootDir, @"SpriteRenderer\Shaders"));

            exchange
                .Register<IFileSystem>(fileSystem)
                .Register<IResourceLayoutSource>(layoutSource)
                .Register<ISpriteSamplerSource>(samplerSource)
                .Register<ITextureSource>(textureSource)
                .Register<IShaderCache>(shaderCache)
                .Register<ICamera>(camera)
                .Attach(framebuffer)
                .Attach(scene)
                .Attach(engine)
                .Attach(paletteManager)
                .Attach(spriteManager)
                .Attach(spriteFactory)
                ;

            var key = new SpriteKey(texture, SpriteSampler.Linear, 1, SpriteKeyFlags.UsePalette);
            var width = 200;
            var height = 100;
            var lease = spriteManager.Borrow(key, 5 * width * height, null);
            for (int j = 0; j < height; j++)
                for (int i = 0; i < width; i++)
                    Set(i, j, width, height, lease, texture);
            engine.Run();
            return 0;
        }

        static void Set(int x, int y, int width, int height, SpriteLease lease, ITexture texture)
        {
            int baseIndex = (y * width + x) * 5;
            var center = new Vector3(
                16.0f*((float)x/width - 0.5f),
                16.0f*((float)y/height - 0.5f),
                -10);

            var size = new Vector2((float)x / (8*width), (float)y / (8*height));
            var size3 = new Vector3(size.X, size.Y, 0);

            bool lockWasTaken = false;
            var span = lease.Lock(ref lockWasTaken);
            try
            {
                span[baseIndex + 0] = new SpriteInstanceData(center + size3 * new Vector3(0, 0, 0), size, texture.Regions[0], SpriteFlags.None);
                span[baseIndex + 1] = new SpriteInstanceData(center + size3 * new Vector3(-1, -1, 0), size, texture.Regions[1], SpriteFlags.None);
                span[baseIndex + 2] = new SpriteInstanceData(center + size3 * new Vector3(-1, 1, 0), size, texture.Regions[2], SpriteFlags.None);
                span[baseIndex + 3] = new SpriteInstanceData(center + size3 * new Vector3(1, -1, 0), size, texture.Regions[3], SpriteFlags.None);
                span[baseIndex + 4] = new SpriteInstanceData(center + size3 * new Vector3(1, 1, 0), size, texture.Regions[4], SpriteFlags.None);
            }
            finally
            {
                lease.Unlock(lockWasTaken);
            }
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
