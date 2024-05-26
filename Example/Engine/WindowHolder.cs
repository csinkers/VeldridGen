using System;
using System.Numerics;
using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;
using VeldridGen.Example.Engine.Events;

namespace VeldridGen.Example.Engine;

class WindowHolder : Component, IDisposable
{
    Sdl2Window _window;
    DateTime _lastTitleUpdateTime;
    Vector2? _pendingCursorUpdate;
    public Sdl2Window Window => _window;
    public string WindowTitle { get; set; }

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
            WindowTitle = "VeldridGen-Example"
        };

        _window = VeldridStartup.CreateWindow(ref windowInfo);
        _window.CursorVisible = true;
        _window.Resized += () => Raise(new WindowResizedEvent(_window.Width, _window.Height));
        _window.Closed += () => Raise(new WindowClosedEvent());
        Raise(new WindowResizedEvent(_window.Width, _window.Height));
    }

    void SetTitle()
    {
        if (DateTime.UtcNow - _lastTitleUpdateTime <= TimeSpan.FromSeconds(1))
            return;

        var engine = Resolve<IEngine>();
        _window.Title = $"{WindowTitle} - {engine.FrameTimeText}";
        _lastTitleUpdateTime = DateTime.UtcNow;
    }

    public void Dispose() => _window.Close();
    public void PumpEvents()
    {
        SetTitle();
        Sdl2Events.ProcessEvents();
        var snapshot = _window.PumpEvents();
        if (_window == null)
            return;

        if (!_pendingCursorUpdate.HasValue || !_window.Focused)
            return;

        using (PerfTracker.FrameEvent("3 Warping mouse"))
        {
            Sdl2Native.SDL_WarpMouseInWindow(
                _window.SdlWindowHandle,
                (int)_pendingCursorUpdate.Value.X,
                (int)_pendingCursorUpdate.Value.Y);

            _pendingCursorUpdate = null;
        }
    }
}