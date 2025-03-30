﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Veldrid;
using VeldridGen.Example.Engine;
using VeldridGen.Example.Engine.Events;
using VeldridGen.Example.Engine.Visual;
using VeldridGen.Example.SpriteRenderer;
using VeldridGen.Interfaces;

namespace VeldridGen.Example.TestApp
{
    public sealed class SceneRenderer : ServiceComponent<ISceneRenderer>, ISceneRenderer, IDisposable
    {
        readonly Dictionary<Type, IRenderer> _rendererLookup = new();
        readonly List<IRenderer> _renderers = [];
        readonly List<IRenderable> _renderList = [];
        readonly List<IRenderableSource> _sources = [];
        readonly SingleBuffer<GlobalInfo> _globalInfo;
        readonly SingleBuffer<ProjectionMatrix> _projection;
        readonly SingleBuffer<ViewMatrix> _view;
        readonly CommonSet _commonSet;
        ITextureHolder _palette;

        public SceneRenderer(string name, IFramebufferHolder framebuffer)
        {
            Name = name;
            Framebuffer = framebuffer ?? throw new ArgumentNullException(nameof(framebuffer));

            On<RenderEvent>(_ => UpdatePerFrameResources());

            _projection = new SingleBuffer<ProjectionMatrix>(BufferUsage.UniformBuffer | BufferUsage.DynamicWrite, "M_Projection");
            _view = new SingleBuffer<ViewMatrix>(BufferUsage.UniformBuffer | BufferUsage.DynamicWrite, "M_View");
            _globalInfo = new SingleBuffer<GlobalInfo>(BufferUsage.UniformBuffer | BufferUsage.DynamicWrite, "B_GlobalInfo");
            _commonSet = new CommonSet
            {
                Name = "RS_Common",
                GlobalInfo = _globalInfo,
                Projection = _projection,
                View = _view,
            };

            AttachChild(_projection);
            AttachChild(_view);
            AttachChild(_globalInfo);
            AttachChild(_commonSet);
        }

        public SceneRenderer AddRenderer(IRenderer renderer, params Type[] types)
        {
            ArgumentNullException.ThrowIfNull(renderer);
            if (!_renderers.Contains(renderer))
            {
                _renderers.Add(renderer);
                AttachChild(renderer);
            }

            foreach (var type in types)
            {
                if (!_rendererLookup.TryAdd(type, renderer))
                {
                    throw new InvalidOperationException(
                        $"Tried to register renderer of type {renderer.GetType().Name} for" +
                        $" rendering \"{type.Name}\", but they are already being handled by " +
                        _rendererLookup[type].GetType().Name);
                }
            }

            return this;
        }

        public SceneRenderer AddSource(IRenderableSource source)
        {
            ArgumentNullException.ThrowIfNull(source);
            _sources.Add(source);
            return this;
        }

        public string Name { get; }
        public IFramebufferHolder Framebuffer { get; }
        public override string ToString() => $"Scene:{Name}";
        public void Render(GraphicsDevice device, CommandList cl)
        {
            ArgumentNullException.ThrowIfNull(device);
            ArgumentNullException.ThrowIfNull(cl);

            // Sort:
            // Opaque, front-to-back (map, then sprites)
            // Transparent, back-to-front (1 call per map-tile / sprite)
            // CoreTrace.Log.Info("Scene", "Sorted processed renderables");

            // Main scene
            using (PerfTracker.FrameEvent("6.2.3 Main scene pass"))
            {
                cl.SetFramebuffer(Framebuffer.Framebuffer);
                cl.SetFullViewports();
                cl.SetFullScissorRects();
                cl.ClearColorTarget(0, new RgbaFloat(0, 0, 0, 1));
                cl.ClearDepthStencil(device.IsDepthRangeZeroToOne ? 1f : 0f);

                _renderList.Clear();
                foreach (var source in _sources)
                    source.Collect(_renderList);

                _renderList.Sort((x, y) => x.RenderOrder.CompareTo(y.RenderOrder));

                foreach (var renderable in _renderList)
                {
                    if (_rendererLookup.TryGetValue(renderable.GetType(), out var renderer))
                        renderer.Render(renderable, _commonSet, Framebuffer, cl, device);
                }
            }
        }

        void UpdatePerFrameResources()
        {
            var camera = Resolve<ICamera>();
            var settings = TryResolve<IEngineSettings>();
            var paletteManager = Resolve<IPaletteManager>();
            var textureSource = Resolve<ITextureSource>();

            camera.Viewport = new Vector2(Framebuffer.Width, Framebuffer.Height);
            var palette = textureSource.GetSimpleTexture(paletteManager.PaletteTexture);
            if (_palette != palette)
            {
                _palette = palette;
                _commonSet.Palette = _palette;
            }

            var info = new GlobalInfo
            {
                WorldSpacePosition = camera.Position,
                CameraDirection = new Vector2(camera.Pitch, camera.Yaw),
                Resolution = new Vector2(Framebuffer.Width, Framebuffer.Height),
                Time = (float)(DateTime.Now - DateTime.UnixEpoch).TotalSeconds,
                EngineFlags = settings?.Flags ?? 0,
            };

            _projection.Data = new ProjectionMatrix(camera.ProjectionMatrix);
            _view.Data = new ViewMatrix(camera.ViewMatrix);
            _globalInfo.Data = info;
        }

        public void Dispose()
        {
            _commonSet?.Dispose();
            _globalInfo.Dispose();
            _projection.Dispose();
            _view.Dispose();

            foreach (var renderer in _renderers.OfType<IDisposable>())
                renderer.Dispose();

            _renderers.Clear();
            _rendererLookup.Clear();
        }
    }
}
