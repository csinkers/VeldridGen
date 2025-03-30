﻿using System;
using Veldrid;
using VeldridGen.Interfaces;

namespace VeldridGen.Example.Engine;

public sealed class Texture2DArrayHolder(string name) : TextureHolder(name), ITextureArrayHolder
{
    protected override void Validate(Texture texture)
    {
        if (texture == null)
            return;
        if (texture.Type != TextureType.Texture2D)
            throw new ArgumentOutOfRangeException($"Tried to assign a {texture.Type} to Texture2DArrayHolder \"{Name}\"");
        if (texture.ArrayLayers < 2)
            throw new ArgumentOutOfRangeException($"Tried to assign a single-layer texture to Texture2DArrayHolder \"{Name}\"");
    }
}
