using System;
using System.Numerics;
using UAlbion.Api.Visual;
using UAlbion.Core.Veldrid;
using UAlbion.Core.Visual;
using Veldrid;

namespace UAlbion.Core.SpriteRenderer
{
    public class SpriteScene : Component, IScene
    {
        readonly ICamera _camera;
        readonly ITexture _palette;
        readonly SpriteManager _spriteManager;
        readonly SpriteRenderer _spriteRenderer;
        readonly FramebufferHolder _framebuffer;
        readonly SingleBuffer<ViewMatrix> _viewMatrix;
        readonly SingleBuffer<ProjectionMatrix> _projectionMatrix;
        readonly SingleBuffer<GlobalInfo> _globalInfo;
        CommonSet _commonSet;

        public SpriteScene(ITexture palette, FramebufferHolder framebuffer)
        {
            _palette = palette ?? throw new ArgumentNullException(nameof(palette));
            _framebuffer = framebuffer ?? throw new ArgumentNullException(nameof(framebuffer));

            _camera = AttachChild(new PerspectiveCamera());
            _spriteRenderer = AttachChild(new SpriteRenderer());
            _globalInfo = AttachChild(new SingleBuffer<GlobalInfo>(BuildGlobalInfo(), BufferUsage.UniformBuffer, "B_Global"));
            _viewMatrix = AttachChild(new SingleBuffer<ViewMatrix>(new ViewMatrix(_camera.ViewMatrix), BufferUsage.UniformBuffer, "M_View"));
            _projectionMatrix = AttachChild(new SingleBuffer<ProjectionMatrix>(new ProjectionMatrix(_camera.ProjectionMatrix), BufferUsage.UniformBuffer, "M_Projection"));
            _spriteManager = AttachChild(new SpriteManager(SpriteBatchComparer.Instance));
        }

        public SpriteManager SpriteManager => _spriteManager;

        protected override void Subscribed()
        {
            var paletteHolder = Resolve<ITextureSource>().GetSimpleTexture(_palette);

            _commonSet ??= AttachChild(new CommonSet
            {
                Name = "RS_Common",
                GlobalInfo = _globalInfo,
                Projection = _projectionMatrix,
                View = _viewMatrix,
                Palette = paletteHolder
            });
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