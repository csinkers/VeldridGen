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

        public void CreateWindow(int x, int y, int width, int height)
        {
            _window?.Close();

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
            _window.CursorVisible = false;
            _window.Resized += () => Raise(new WindowResizedEvent(width, height));
            _window.Closed += () => Raise(new QuitEvent());
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