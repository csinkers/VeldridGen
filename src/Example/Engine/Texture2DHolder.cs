using System;
using Veldrid;
using VeldridGen.Interfaces;

namespace VeldridGen.Example.Engine;

public sealed class Texture2DHolder(string name) : TextureHolder(name), ITextureHolder
{
    protected override void Validate(Texture texture)
    {
        if (texture == null)
            return;
        if (texture.Type != TextureType.Texture2D)
            throw new ArgumentOutOfRangeException($"Tried to assign a {texture.Type} to Texture2DHolder \"{Name}\"");
        if (texture.ArrayLayers > 1)
            throw new ArgumentOutOfRangeException($"Tried to assign a multi-layer texture to Texture2DHolder \"{Name}\"");
    }
}
