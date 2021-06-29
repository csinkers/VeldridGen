using System;
using System.Diagnostics;
using ImGuiNET;
using UAlbion.Api;
using UAlbion.Core.Events;
using UAlbion.Core.Veldrid.Events;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using Rectangle = Veldrid.Rectangle;

namespace UAlbion.Core.Veldrid
{
    public class Engine : Component
    {
        static RenderDoc _renderDoc;

        readonly WindowManager _windowManager;
        readonly bool _useRenderDoc;
        readonly bool _vsync;
        readonly int _defaultWidth = 720;
        readonly int _defaultHeight = 480;
        readonly int _defaultX = 648;
        readonly int _defaultY = 431;
        readonly IScene _scene;

        GraphicsDevice _graphicsDevice;
        CommandList _frameCommands;
        GraphicsBackend? _newBackend;
        bool _done;

        public Engine(GraphicsBackend backend, bool useRenderDoc, bool vsync, IScene scene, Rectangle? windowRect = null)
        {
            _windowManager = AttachChild(new WindowManager());

            _newBackend = backend;
            _useRenderDoc = useRenderDoc;
            _vsync = vsync;
            _scene = scene;

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
            On<WindowResizedEvent>(e => _graphicsDevice.ResizeMainWindow((uint)e.Width, (uint)e.Height));
            /*
            On<LoadRenderDocEvent>(e =>
            {
                if (_renderDoc != null || !RenderDoc.Load(out _renderDoc)) return;
                _newBackend = GraphicsDevice.BackendType;
                _recreateWindow = true;
            });
            On<RunRenderDocEvent>(e => _renderDoc?.LaunchReplayUI());
            On<TriggerRenderDocEvent>(e => _renderDoc?.TriggerCapture());
            On<SetBackendEvent>(e => _newBackend = e.Value);
            On<SetVSyncEvent>(e =>
            {
                if (_vsync == e.Value) return;
                _vsync = e.Value;
                _newBackend = GraphicsDevice.BackendType;
            });
            */
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
                _graphicsDevice.Dispose();
            }

            PerfTracker.StartupEvent("Startup done, rendering first frame");
            Resolve<IShaderCache>()?.CleanupOldFiles();
        }

        void InnerLoop()
        {
            if (_graphicsDevice == null)
                throw new InvalidOperationException("GraphicsDevice not initialised");

            var frameCounter = Stopwatch.StartNew();
            while (!_done && _newBackend == null)
            {
                var deltaSeconds = frameCounter.Elapsed.TotalSeconds;
                frameCounter.Restart();

                PerfTracker.BeginFrame();
                using (PerfTracker.FrameEvent("1 Raising begin frame"))
                    Raise(BeginFrameEvent.Instance);

                Sdl2Events.ProcessEvents();
                _windowManager.PumpEvents();

                using (PerfTracker.FrameEvent("5 Performing update"))
                    Raise(new EngineUpdateEvent(deltaSeconds));

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
                Raise(new PrepareFrameResourcesEvent(_graphicsDevice, _frameCommands));
                Raise(new PrepareFrameResourceSetsEvent(_graphicsDevice, _frameCommands));
                _frameCommands.End();
                _graphicsDevice.SubmitCommands(_frameCommands);
            }

            using (PerfTracker.FrameEvent("6.2 Render scenes"))
            {
                _frameCommands.Begin();
                _scene.Render(_graphicsDevice, _frameCommands);
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
                        throw new InvalidOperationException("Failed to load renderdoc");
                }

                _renderDoc.APIValidation = true;
            }

            _windowManager.CreateWindow(_defaultX, _defaultY, _defaultWidth, _defaultHeight);
            GraphicsDeviceOptions gdOptions = new GraphicsDeviceOptions(
                _renderDoc != null, PixelFormat.R32_Float, false,
                ResourceBindingModel.Improved, true,
                true, false)
            {
                SyncToVerticalBlank = _vsync,
#if DEBUG
                Debug = true
#endif
            };

            // Currently this field only exists in my local build of veldrid, so set it via reflection.
            var singleThreadedProperty = typeof(GraphicsDeviceOptions).GetField("SingleThreaded");
            if (singleThreadedProperty != null)
                singleThreadedProperty.SetValueDirect(__makeref(gdOptions), true);

            _graphicsDevice = VeldridStartup.CreateGraphicsDevice(_windowManager.Window, gdOptions, backend);
            _graphicsDevice.WaitForIdle();

            _frameCommands = _graphicsDevice.ResourceFactory.CreateCommandList();
            _frameCommands.Name = "Frame Commands List";

            Raise(new DeviceCreatedEvent(_graphicsDevice));
        }

        void DestroyAllObjects()
        {
            using (PerfTracker.InfrequentEvent("Destroying objects"))
            {
                Raise(new DestroyDeviceObjectsEvent());
                _graphicsDevice?.WaitForIdle();
                _frameCommands?.Dispose();
                _frameCommands = null;

                _graphicsDevice?.WaitForIdle();
            }
        }
    }
}
