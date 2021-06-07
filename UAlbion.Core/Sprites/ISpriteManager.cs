namespace UAlbion.Core.Sprites
{
    public interface ISpriteManager
    {
        ISpriteLease Borrow(SpriteKey key, int length, object caller);
        void Cleanup();
    }
}