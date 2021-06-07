using System;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace UAlbion.Core.Veldrid
{
    class WindowManager : Component, IDisposable
    {
        bool _windowResized;
        bool _recreateWindow;
        Sdl2Window _window;
        public Sdl2Window Window => _window;

        void Foo()
        {
            /*
            if (_windowResized)
            {
                GraphicsDevice.ResizeMainWindow((uint)width, (uint)height);
                Raise(new WindowResizedEvent(width, height));
                CoreTrace.Log.Info("Engine", "Resize finished");
                _windowResized = false;
            }*/
        }

        public void CreateWindow(int x, int y, int width, int height)
        {
            // if (_showWindow && (_window == null || _recreateWindow))

            _recreateWindow = false;
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
            //Window.BorderVisible = false;
            _window.CursorVisible = false;
            _window.Resized += () => _windowResized = true;
            // _window.FocusGained += () => Raise(new FocusGainedEvent());
            // _window.FocusLost += () => Raise(new FocusLostEvent());
            _windowResized = true;
        }

        void ToggleFullscreenState()
        {
            if (_window == null)
                return;
            bool isFullscreen = _window.WindowState == WindowState.BorderlessFullScreen;
            _window.WindowState = isFullscreen ? WindowState.Normal : WindowState.BorderlessFullScreen;
        }

        public void Dispose()
        {
            _window.Close();
        }

        public void PumpEvents()
        {
            _window.PumpEvents();
        }
    }
}