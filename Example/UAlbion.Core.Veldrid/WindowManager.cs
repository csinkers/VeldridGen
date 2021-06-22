using System;
using UAlbion.Core.Veldrid.Events;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace UAlbion.Core.Veldrid
{
    class WindowManager : Component, IDisposable
    {
        Sdl2Window _window;
        public Sdl2Window Window => _window;

        public WindowManager()
        {
            /*
            On<SetCursorPositionEvent>(e => _pendingCursorUpdate = new Vector2(e.X, e.Y));
            On<ToggleFullscreenEvent>(e => ToggleFullscreenState());
            On<ToggleHardwareCursorEvent>(e => { if (_window != null) _window.CursorVisible = !_window.CursorVisible; });
            On<ToggleResizableEvent>(e => { if (_window != null) _window.Resizable = !_window.Resizable; });
            On<ToggleVisibleBorderEvent>(e => { if (_window != null) _window.BorderVisible = !_window.BorderVisible; });
            On<RecreateWindowEvent>(e => { _recreateWindow = true; _newBackend = GraphicsDevice.BackendType; });
            On<ConfineMouseToWindowEvent>(e => { if (_window != null) Sdl2Native.SDL_SetWindowGrab(_window.SdlWindowHandle, e.Enabled); });
            On<SetRelativeMouseModeEvent>(e =>
            {
                if (_window == null) return;
                Sdl2Native.SDL_SetRelativeMouseMode(e.Enabled);
                if (!e.Enabled)
                    Sdl2Native.SDL_WarpMouseInWindow(_window.SdlWindowHandle, _window.Width / 2, _window.Height / 2);
            }); */
        }

        public void CreateWindow(int x, int y, int width, int height)
        {
            if (_window != null)
                return;

            var windowInfo = new WindowCreateInfo
            {
                X = x,
                Y = y,
                WindowWidth = _window?.Width ?? width,
                WindowHeight = _window?.Height ?? height,
                WindowInitialState = _window?.WindowState ?? WindowState.Normal,
                WindowTitle = "UAlbion"
            };

            _window = VeldridStartup.CreateWindow(ref windowInfo);
            // _window.CursorVisible = false;
            _window.Resized += () => Raise(new WindowResizedEvent(width, height));
            _window.Closed += () => Raise(new WindowClosedEvent());
            // _window.FocusGained += () => Raise(new FocusGainedEvent());
            // _window.FocusLost += () => Raise(new FocusLostEvent());
        }

        void ToggleFullscreenState()
        {
            if (_window == null)
                return;
            bool isFullscreen = _window.WindowState == WindowState.BorderlessFullScreen;
            _window.WindowState = isFullscreen ? WindowState.Normal : WindowState.BorderlessFullScreen;
        }

        public void Dispose() => _window.Close();
        public void PumpEvents() => _window.PumpEvents();
    }
}