using System;
using ImGuiNET;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using VeldridGen.Example.Engine.Events;
using Rectangle = Veldrid.Rectangle;

namespace VeldridGen.Example.Engine
{
    public sealed class VeldridEngine : ServiceComponent<IEngine>, IEngine, IDisposable
    {
        static RenderDoc _renderDoc;

        readonly FrameTimeAverager _frameTimeAverager = new(0.5);
        readonly WindowHolder _windowHolder;
        readonly bool _useRenderDoc;
        readonly int _defaultWidth = 720;
        readonly int _defaultHeight = 480;
        readonly int _defaultX = 648;
        readonly int _defaultY = 431;
        readonly ISceneRenderer _sceneRenderer;

        GraphicsDevice _graphicsDevice;
        CommandList _frameCommands;
        GraphicsBackend? _newBackend;
        bool _done;

        public bool IsDepthRangeZeroToOne => _graphicsDevice?.IsDepthRangeZeroToOne ?? false;
        public bool IsClipSpaceYInverted => _graphicsDevice?.IsClipSpaceYInverted ?? false;
        public string FrameTimeText => $"{_graphicsDevice.BackendType} {_frameTimeAverager.CurrentAverageFramesPerSecond:N2} fps ({_frameTimeAverager.CurrentAverageFrameTimeMilliseconds:N3} ms)";

        public VeldridEngine(GraphicsBackend backend, bool useRenderDoc, ISceneRenderer sceneRenderer, Rectangle? windowRect = null)
        {
            _newBackend = backend;
            _useRenderDoc = useRenderDoc;
            _sceneRenderer = sceneRenderer ?? throw new ArgumentNullException(nameof(sceneRenderer));
            _windowHolder = new WindowHolder();
            AttachChild(_windowHolder);

            if (windowRect.HasValue)
            {
                _defaultX = windowRect.Value.X;
                _defaultY = windowRect.Value.Y;
                _defaultWidth = windowRect.Value.Width;
                _defaultHeight = windowRect.Value.Height;
            }

            On<WindowClosedEvent>(_ =>
            {
                if (_newBackend == null)
                    _done = true;
            });
            On<QuitEvent>(_ => _done = true);
            On<WindowResizedEvent>(e =>
            {
                _graphicsDevice?.ResizeMainWindow((uint)e.Width, (uint)e.Height);
            });
        }

        protected override void Subscribed()
        {
            Resolve<IShaderCache>().ShadersUpdated += OnShadersUpdated;
            base.Subscribed();
        }

        protected override void Unsubscribed()
        {
            Resolve<IShaderCache>().ShadersUpdated -= OnShadersUpdated;
            base.Unsubscribed();
        }

        void OnShadersUpdated(object _, EventArgs eventArgs) => _newBackend = _graphicsDevice?.BackendType;

        public void Run()
        {
            PerfTracker.StartupEvent("Set up backend");
            Sdl2Native.SDL_Init(SDLInitFlags.GameController);

            if (ImGui.GetCurrentContext() != IntPtr.Zero)
            {
                ImGui.StyleColorsClassic();

                // Turn on ImGui docking if it's supported
                if (Enum.TryParse(typeof(ImGuiConfigFlags), "DockingEnable", out var dockingFlag) && dockingFlag != null)
                    ImGui.GetIO().ConfigFlags |= (ImGuiConfigFlags)dockingFlag;
            }

            while (!_done)
            {
                GraphicsBackend backend = _newBackend ?? _graphicsDevice.BackendType;
                using (PerfTracker.InfrequentEvent($"change backend to {backend}"))
                    ChangeBackend(backend);
                _newBackend = null;

                InnerLoop();
                DestroyAllObjects();
            }

            Resolve<IShaderCache>()?.CleanupOldFiles();
        }

        void InnerLoop()
        {
            if (_graphicsDevice == null)
                throw new InvalidOperationException(Errors.DeviceUninitialized);

            var frameCounter = new FrameCounter();
            while (!_done && _newBackend == null)
            {
                var deltaSeconds = frameCounter.StartFrame();
                _frameTimeAverager.AddTime(deltaSeconds);

                PerfTracker.BeginFrame();
                using (PerfTracker.FrameEvent("1 Raising begin frame"))
                    Raise(BeginFrameEvent.Instance);

                using (PerfTracker.FrameEvent("2 Processing SDL events"))
                {
                    Sdl2Events.ProcessEvents();
                    _windowHolder.PumpEvents();
                }

                using (PerfTracker.FrameEvent("5 Performing update"))
                    Raise(new EngineUpdateEvent((float)deltaSeconds));

                using (PerfTracker.FrameEvent("5.1 Flushing queued events"))
                    Exchange.FlushQueuedEvents();

                using (PerfTracker.FrameEvent("6 Drawing"))
                    Draw();

                using (PerfTracker.FrameEvent("7 Swap buffers"))
                {
                    CoreTrace.Log.Info("Engine", "Swapping buffers...");
                    _graphicsDevice.SwapBuffers();
                    CoreTrace.Log.Info("Engine", "Draw complete");
                }
            }
        }

        void Draw()
        {
            using (PerfTracker.FrameEvent("6.1 Prepare scenes"))
            {
                _frameCommands.Begin();
                Raise(new RenderEvent(_graphicsDevice, _frameCommands));
                Raise(new PrepareFrameResourcesEvent(_graphicsDevice, _frameCommands));
                Raise(new PrepareFrameResourceSetsEvent(_graphicsDevice, _frameCommands));
                _frameCommands.End();
                _graphicsDevice.SubmitCommands(_frameCommands);
            }

            using (PerfTracker.FrameEvent("6.2 Render scenes"))
            {
                _frameCommands.Begin();
                _sceneRenderer.Render(_graphicsDevice, _frameCommands);
                _frameCommands.End();
            }

            using (PerfTracker.FrameEvent("6.3 Submit commandlist"))
            {
                CoreTrace.Log.Info("Scene", "Submitting commands");
                _graphicsDevice.SubmitCommands(_frameCommands);
                CoreTrace.Log.Info("Scene", "Submitted commands");
                _graphicsDevice.WaitForIdle();
            }
        }

        void ChangeBackend(GraphicsBackend backend)
        {
            if (_useRenderDoc)
            {
                using (PerfTracker.InfrequentEvent("Loading renderdoc"))
                {
                    if (!RenderDoc.Load(out _renderDoc))
                        throw new InvalidOperationException(Errors.RenderDocFail);
                }

                _renderDoc.APIValidation = true;
            }

            var gdOptions = new GraphicsDeviceOptions(
                _renderDoc != null,
                PixelFormat.R32_Float,
                false,
                ResourceBindingModel.Improved,
                true,
                true,
                false)
            {
#if DEBUG
                Debug = true
#endif
            };

            _windowHolder.CreateWindow(_defaultX, _defaultY, _defaultWidth, _defaultHeight);
            _graphicsDevice = VeldridStartup.CreateGraphicsDevice(_windowHolder.Window, gdOptions, backend);
            _graphicsDevice.WaitForIdle();

            _frameCommands = _graphicsDevice.ResourceFactory.CreateCommandList();
            _frameCommands.Name = "Frame Commands List";

            Raise(new DeviceCreatedEvent(_graphicsDevice));
            Raise(new BackendChangedEvent());
        }

        void DestroyAllObjects()
        {
            using (PerfTracker.InfrequentEvent("Destroying objects"))
            {
                Raise(new DestroyDeviceObjectsEvent());
                _graphicsDevice?.WaitForIdle();
                _frameCommands?.Dispose();
                _graphicsDevice?.Dispose();
                _frameCommands = null;
                _graphicsDevice = null;
            }
        }

        public void Dispose()
        {
            _windowHolder?.Dispose();
            _graphicsDevice?.Dispose();
            _frameCommands?.Dispose();
        }
    }
}
