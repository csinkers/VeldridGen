using System;

namespace UAlbion.Core.Sprites
{
    /// <summary>
    /// Flags that need to be the same for all instances in a sprite draw call
    /// </summary>
    [Flags]
    public enum SpriteKeyFlags
    {
        NoDepthTest    = 0x1,
        NoTransform    = 0x2,
        UseCylindrical = 0x4
    }
}
