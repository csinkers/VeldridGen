using System;
using VeldridGen.Example.Engine;
using VeldridGen.Example.Engine.Visual;

namespace VeldridGen.Example.TestApp
{
    public class PaletteManager : ServiceComponent<IPaletteManager>, IPaletteManager
    {
        public PaletteManager(ITexture paletteTexture) => PaletteTexture = paletteTexture ?? throw new ArgumentNullException(nameof(paletteTexture));
        public ITexture PaletteTexture { get; }
    }
}