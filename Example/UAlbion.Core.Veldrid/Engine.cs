using System;
using System.Diagnostics;
using ImGuiNET;
using UAlbion.Api;
using UAlbion.Core.Events;
using UAlbion.Core.Veldrid.Events;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using VeldridGen.Interfaces;
using Rectangle = Veldrid.Rectangle;

namespace UAlbion.Core.Veldrid
{
    public class Engine : Component
    {
        static RenderDoc _renderDoc;

        readonly WindowManager _windowManager;
        readonly bool _useRenderDoc;
        readonly int _defaultWidth = 720;
        readonly int _defaultHeight = 480;
        readonly int _defaultX = 648;
        readonly int _defaultY = 431;
        readonly IScene _scene;

        CommandList _frameCommands;
        bool _done;
        bool _vsync;// = true;
        GraphicsBackend? _newBackend;

        internal GraphicsDevice GraphicsDevice { get; private set; }
        internal static RenderDoc RenderDoc => _renderDoc;
        public string WindowTitle { get; set; }

        public Engine(GraphicsBackend backend, bool useRenderDoc, IScene scene, Rectangle? windowRect = null)
        {
            On<WindowClosedEvent>(_ =>
            {
                if (_newBackend == null)
                    _done = true;
            });
            On<QuitEvent>(e => _done = true);
            On<WindowResizedEvent>(e => GraphicsDevice.ResizeMainWindow((uint)e.Width, (uint)e.Height));

            /*
            On<LoadRenderDocEvent>(e =>
            {
                if (_renderDoc != null || !RenderDoc.Load(out _renderDoc)) return;
                _newBackend = GraphicsDevice.BackendType;
                _recreateWindow = true;
            });
            On<GarbageCollectionEvent>(e => GC.Collect());
            On<RunRenderDocEvent>(e => _renderDoc?.LaunchReplayUI());
            On<SetCursorPositionEvent>(e => _pendingCursorUpdate = new Vector2(e.X, e.Y));
            On<ToggleFullscreenEvent>(e => ToggleFullscreenState());
            On<ToggleHardwareCursorEvent>(e => { if (_window != null) _window.CursorVisible = !_window.CursorVisible; });
            On<ToggleResizableEvent>(e => { if (_window != null) _window.Resizable = !_window.Resizable; });
            On<ToggleVisibleBorderEvent>(e => { if (_window != null) _window.BorderVisible = !_window.BorderVisible; });
            On<RecreateWindowEvent>(e => { _recreateWindow = true; _newBackend = GraphicsDevice.BackendType; });
            On<SetBackendEvent>(e => _newBackend = e.Value);
            On<TriggerRenderDocEvent>(e => _renderDoc?.TriggerCapture());
            On<ConfineMouseToWindowEvent>(e => { if (_window != null) Sdl2Native.SDL_SetWindowGrab(_window.SdlWindowHandle, e.Enabled); });
            On<SetRelativeMouseModeEvent>(e =>
            {
                if (_window == null) return;
                Sdl2Native.SDL_SetRelativeMouseMode(e.Enabled);
                if (!e.Enabled)
                    Sdl2Native.SDL_WarpMouseInWindow(_window.SdlWindowHandle, _window.Width / 2, _window.Height / 2);
            });
            On<SetVSyncEvent>(e =>
            {
                if (_vsync == e.Value) return;
                _vsync = e.Value;
                _newBackend = GraphicsDevice.BackendType;
            });
            */

            _windowManager = AttachChild(new WindowManager());

            _newBackend = backend;
            _useRenderDoc = useRenderDoc;
            _scene = scene;

            if (windowRect.HasValue)
            {
                _defaultX = windowRect.Value.X;
                _defaultY = windowRect.Value.Y;
                _defaultWidth = windowRect.Value.Width;
                _defaultHeight = windowRect.Value.Height;
            }
        }

        protected override void Subscribed()
        {
            var shaderCache = Resolve<IShaderCache>();
            if(shaderCache == null)
                throw new InvalidOperationException("An instance of IShaderCache must be registered.");
            shaderCache.ShadersUpdated += (_, _) => _newBackend = GraphicsDevice?.BackendType;
            base.Subscribed();
        }

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
                GraphicsBackend backend = _newBackend ?? GraphicsDevice.BackendType;
                using (PerfTracker.InfrequentEvent($"change backend to {backend}"))
                    ChangeBackend(backend);
                _newBackend = null;

                InnerLoop();

                DestroyAllObjects();
                GraphicsDevice.Dispose();
            }

            PerfTracker.StartupEvent("Startup done, rendering first frame");
            Resolve<IShaderCache>()?.CleanupOldFiles();
        }

        void InnerLoop()
        {
            if (GraphicsDevice == null)
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
                    GraphicsDevice.SwapBuffers();
                    CoreTrace.Log.Info("Engine", "Draw complete");
                }
            }
        }

        void Draw()
        {
            using (PerfTracker.FrameEvent("6.1 Prepare scenes"))
            {
                _frameCommands.Begin();
                Raise(new PrepareFrameResourcesEvent(GraphicsDevice, _frameCommands));
                Raise(new PrepareFrameResourceSetsEvent(GraphicsDevice, _frameCommands));
                _frameCommands.End();
                GraphicsDevice.SubmitCommands(_frameCommands);
            }

            using (PerfTracker.FrameEvent("6.2 Render scenes"))
            {
                _frameCommands.Begin();
                _scene.Render(GraphicsDevice, _frameCommands);
                _frameCommands.End();
            }

            using (PerfTracker.FrameEvent("6.3 Submit commandlist"))
            {
                CoreTrace.Log.Info("Scene", "Submitting commands");
                GraphicsDevice.SubmitCommands(_frameCommands);
                CoreTrace.Log.Info("Scene", "Submitted commands");
                GraphicsDevice.WaitForIdle();
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

            GraphicsDevice = VeldridStartup.CreateGraphicsDevice(_windowManager.Window, gdOptions, backend);
            GraphicsDevice.WaitForIdle();

            _frameCommands = GraphicsDevice.ResourceFactory.CreateCommandList();
            _frameCommands.Name = "Frame Commands List";

            Raise(new DeviceCreatedEvent());
        }

        void DestroyAllObjects()
        {
            using (PerfTracker.InfrequentEvent("Destroying objects"))
            {
                Raise(new DestroyDeviceObjectsEvent());
                GraphicsDevice?.WaitForIdle();
                _frameCommands?.Dispose();
                _frameCommands = null;

                GraphicsDevice?.WaitForIdle();
            }
        }
    }
}
