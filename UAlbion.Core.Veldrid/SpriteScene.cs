using System;
using System.Numerics;
using UAlbion.Api.Visual;
using UAlbion.Core.Veldrid.SpriteBatch;
using Veldrid;

namespace UAlbion.Core.Veldrid
{
    public class SpriteScene : Component, IScene
    {
        readonly SpriteRenderer _spriteRenderer;
        readonly FramebufferHolder _framebuffer;
        readonly ICamera _camera;
        readonly ITexture _palette;
        CommonSet _commonSet;
        SpriteManager _spriteManager;

        public SpriteScene(ITexture palette, FramebufferHolder framebuffer)
        {
            _camera = AttachChild(new PerspectiveCamera());
            _palette = palette ?? throw new ArgumentNullException(nameof(palette));
            _framebuffer = framebuffer ?? throw new ArgumentNullException(nameof(framebuffer));
            _spriteRenderer = AttachChild(new SpriteRenderer());
        }

        public SpriteManager SpriteManager => _spriteManager;

        protected override void Subscribed()
        {
            var globalInfo = AttachChild(new SingleBuffer<GlobalInfo>(BuildGlobalInfo(), BufferUsage.UniformBuffer, "B_Global"));
            var paletteHolder = Resolve<ITextureSource>().GetSimpleTexture(_palette);
            _commonSet = AttachChild(new CommonSet
            {
                Name = "RS_Common",
                GlobalInfo = globalInfo,
                Projection = _camera.ProjectionMatrix,
                View = _camera.ViewMatrix,
                Palette = paletteHolder
            });
            _spriteManager = AttachChild(new SpriteManager(MultiSpriteComparer.Instance));
        }

        protected override void Unsubscribed()
        {
            _commonSet.Dispose();
            RemoveChild(_commonSet);
        }

        GlobalInfo BuildGlobalInfo()
        {
            var info = new GlobalInfo
            {
                WorldSpacePosition = _camera.Position,
                CameraDirection = _camera.LookDirection,
                Resolution = new Vector2(_framebuffer.Width, _framebuffer.Height),
                Time = 0, // clock?.ElapsedTime ?? 0,
                EngineFlags = 0, // (uint?)settings?.Flags ?? 0,
                PaletteBlend = 0, // paletteManager.PaletteBlend
            };
            return info;
        }

        public void Render(GraphicsDevice device, CommandList cl)
        {
            _commonSet.GlobalInfo.Data = BuildGlobalInfo();

            // var camera = Resolve<ICamera>(); // Only on dirty fb dims
            // camera.Viewport = new Vector2(_framebuffer.Width, _framebuffer.Height);
            foreach (var batch in _spriteManager.Ordered)
            {
                _spriteRenderer.Render(batch, _commonSet, _framebuffer, cl);
            }
        }
    }
}