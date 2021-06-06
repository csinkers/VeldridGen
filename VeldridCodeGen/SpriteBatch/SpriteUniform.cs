using System.Runtime.InteropServices;
using UAlbion.CodeGen;

namespace UAlbion.Core.SpriteBatch
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SpriteUniform  : IUniformFormat // Length must be multiple of 16
    {
        [Uniform("uFlags")] public SpriteKeyFlags Flags { get; set; } // 4 bytes
        [Uniform("uTexSizeW")] public float TextureWidth { get; set; } // 4 bytes
        [Uniform("uTexSizeH")] public float TextureHeight { get; set; } // 4 bytes
    }
}