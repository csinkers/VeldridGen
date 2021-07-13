using VeldridGen.Interfaces;

namespace VeldridGen.Example.Engine
{
    public sealed class Texture2DHolder : TextureHolder, ITextureHolder
    {
        public Texture2DHolder(string name) : base(name) { }
    }
}
