using VeldridGen.Example.Engine;
using VeldridGen.Example.Engine.Visual.Sprites;

namespace VeldridGen.Example.SpriteRenderer
{
    public class SpriteFactory : ServiceComponent<ISpriteFactory>, ISpriteFactory
    {
        public SpriteBatch CreateSpriteBatch(SpriteKey key) 
            => new VeldridSpriteBatch(key);
    }
}