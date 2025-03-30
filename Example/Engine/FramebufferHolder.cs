﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using Veldrid;
using VeldridGen.Example.Engine.Events;
using VeldridGen.Interfaces;

namespace VeldridGen.Example.Engine;

public class RefreshFramebuffersEvent : IEvent;
public abstract class FramebufferHolder : Component, IFramebufferHolder
{
    class TextureHolder(Func<Texture> getter) : ITextureHolder
    {
        readonly Func<Texture> _getter = getter ?? throw new ArgumentNullException(nameof(getter));
        public event PropertyChangedEventHandler PropertyChanged { add { } remove { } }
        public Texture DeviceTexture => _getter();
        public string Name => _getter()?.Name;
    }

    readonly Lock _syncRoot = new();
    readonly List<TextureHolder> _colorHolders = new();
    Framebuffer _framebuffer;
    TextureHolder _depthHolder;
    uint _width;
    uint _height;

    protected FramebufferHolder(string name, uint width, uint height)
    {
        On<DeviceCreatedEvent>(e => Update(e.Device));
        On<DestroyDeviceObjectsEvent>(_ => Dispose());
        On<RefreshFramebuffersEvent>(_ =>
        {
            Dispose();
            Dirty();
        });

        _width = width;
        _height = height;
        Name = name;
    }

    public string Name { get; }

    public uint Width
    {
        get => _width;
        set
        {
            if (_width == value)
                return;

            if (value == 0)
                throw new InvalidOperationException("Tried to resize framebuffer width to 0");

            _width = value;
            Dirty();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Width)));
        }
    }

    public uint Height
    {
        get => _height;
        set
        {
            if (_height == value)
                return;

            if (value == 0)
                throw new InvalidOperationException("Tried to resize framebuffer width to 0");

            _height = value;
            Dirty();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Height)));
        }
    }

    public Framebuffer Framebuffer
    {
        get => _framebuffer;
        protected set
        {
            _framebuffer = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Framebuffer)));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected override void Subscribed() => Dirty();
    protected override void Unsubscribed() => Dispose();
    protected abstract Framebuffer CreateFramebuffer(GraphicsDevice device);
    public abstract OutputDescription? OutputDescription { get; }

    public ITextureHolder DepthTexture
    {
        get
        {
            lock (_syncRoot)
                return _depthHolder ??= new TextureHolder(() => Framebuffer.DepthTarget?.Target);
        }
    }

    public ITextureHolder GetColorTexture(int index)
    {
        lock (_syncRoot)
        {
            while (_colorHolders.Count <= index)
                _colorHolders.Add(new TextureHolder(() => Framebuffer.ColorTargets[index].Target));
            return _colorHolders[index];
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        Framebuffer?.Dispose();
        Framebuffer = null;
    }

    void Dirty() => On<PrepareFrameResourcesEvent>(e => Update(e.Device));
    void Update(GraphicsDevice device)
    {
        Dispose();
        Framebuffer = CreateFramebuffer(device);
        if (Framebuffer != null)
        {
            _width = Framebuffer.Width;
            _height = Framebuffer.Height;
            Framebuffer.Name = Name;
        }

        Off<PrepareFrameResourcesEvent>();
    }
}
