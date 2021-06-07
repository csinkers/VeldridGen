using System.Runtime.InteropServices;
using UAlbion.Core.Sprites;
using VeldridCodeGen.Interfaces;

namespace UAlbion.Core.Veldrid.SpriteBatch
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SpriteUniform  : IUniformFormat // Length must be multiple of 16
    {
        [Uniform("uFlags")] public SpriteKeyFlags Flags { get; set; } // 4 bytes
        [Uniform("uTexSizeW")] public float TextureWidth { get; set; } // 4 bytes
        [Uniform("uTexSizeH")] public float TextureHeight { get; set; } // 4 bytes
        [Uniform("_pad1")] uint Padding { get; set; } // 4 bytes
    }
}