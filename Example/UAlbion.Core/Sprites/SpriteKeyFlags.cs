using System;

namespace UAlbion.Core.Sprites
{
    /// <summary>
    /// Flags that need to be the same for all instances in a sprite draw call
    /// </summary>
    [Flags]
    public enum SpriteKeyFlags
    {
        NoDepthTest     = 0x1,
        UseArrayTexture = 0x2,
        UsePalette      = 0x4,
        NoTransform     = 0x8,
    }
}
