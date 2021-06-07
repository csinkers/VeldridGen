using System;
using UAlbion.Core.Sprites;

namespace UAlbion.Core.Veldrid.SpriteBatch
{
    public class WeakSpriteReference
    {
        readonly WeakReference<SpriteLease> _lease;
        readonly SpriteBatch _spriteBatch;
        readonly int _offset;

        public WeakSpriteReference(SpriteBatch spriteBatch, SpriteLease lease, int offset)
        {
            _spriteBatch = spriteBatch;
            _lease = new WeakReference<SpriteLease>(lease);
            _offset = offset;
        }

        public SpriteInstanceData? Data
        {
            get
            {
                if (_spriteBatch == null ||
                    _lease == null ||
                    !_lease.TryGetTarget(out var lease) ||
                    lease.Disposed)
                {
                    return null;
                }

                return _spriteBatch.Instances.Data[lease.From + _offset];
            }
        }
    }
}