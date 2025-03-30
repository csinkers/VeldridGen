using System;

namespace VeldridGen.Example.Engine.Visual.Sprites;

public class WeakSpriteReference(SpriteBatch spriteBatch, SpriteLease lease, int offset)
{
    readonly WeakReference<SpriteLease> _lease = new(lease);

    public SpriteInstanceData? Data
    {
        get
        {
            if (spriteBatch == null ||
                _lease == null ||
                !_lease.TryGetTarget(out var lease) ||
                lease.Disposed)
            {
                return null;
            }

            bool lockWasTaken = false;
            var span = lease.Lock(ref lockWasTaken);
            try { return span[offset]; }
            finally { lease.Unlock(lockWasTaken); }
        }
    }
}
