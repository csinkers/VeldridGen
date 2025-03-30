using VeldridGen.Example.Engine.Visual;

namespace VeldridGen.Example.Engine;

public record TextureDirtyEvent(ITexture Texture) : IVerboseEvent;
