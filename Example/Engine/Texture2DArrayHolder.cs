using VeldridGen.Interfaces;

namespace VeldridGen.Example.Engine
{
    public sealed class Texture2DArrayHolder : TextureHolder, ITextureArrayHolder
    {
        public Texture2DArrayHolder(string name) : base(name) { }
    }
}
