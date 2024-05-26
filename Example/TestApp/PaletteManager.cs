using System;
using VeldridGen.Example.Engine;
using VeldridGen.Example.Engine.Visual;

namespace VeldridGen.Example.TestApp
{
    public class PaletteManager(ITexture paletteTexture) : ServiceComponent<IPaletteManager>, IPaletteManager
    {
        public ITexture PaletteTexture { get; } = paletteTexture ?? throw new ArgumentNullException(nameof(paletteTexture));
    }
}