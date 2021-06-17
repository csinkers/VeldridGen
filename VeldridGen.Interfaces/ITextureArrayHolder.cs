﻿using System;
using System.ComponentModel;
using Veldrid;

namespace VeldridGen.Interfaces
{
    public interface ITextureArrayHolder : IDisposable, INotifyPropertyChanged
    {
        public Texture DeviceTexture { get; }
        public TextureView TextureView { get; }
    }
}