using System;
using System.Collections.Generic;

namespace UAlbion.Core.SpriteBatch
{
    public class SpriteManager : Component, ISpriteManager
    {
        readonly object _syncRoot = new();
        readonly IDictionary<SpriteKey, MultiSprite> _sprites = new Dictionary<SpriteKey, MultiSprite>();
        readonly List<MultiSprite> _ordered = new();
        readonly IComparer<MultiSprite> _comparer;

        public SpriteManager(IComparer<MultiSprite> comparer)
        {
            _comparer = comparer;
        }

        public SpriteLease Borrow(SpriteKey key, int length, object caller)
        {
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));
            lock (_syncRoot)
            {
                if (!_sprites.TryGetValue(key, out var entry))
                {
                    entry = new MultiSprite(key);
                    // TryResolve<IEngine>()?.RegisterRenderable(entry);
                    _sprites[key] = entry;
                    _ordered.Add(entry);
                }

                return entry.Grow(length, caller);
            }
        }

        public IReadOnlyList<MultiSprite> Ordered { get { _ordered.Sort(_comparer); return _ordered; } }

        public void Cleanup()
        {
            lock (_syncRoot)
            {
                var spritesToRemove = new List<KeyValuePair<SpriteKey, MultiSprite>>();
                foreach (var kvp in _sprites)
                    if (kvp.Value.ActiveInstances == 0)
                        spritesToRemove.Add(kvp);

                foreach (var kvp in spritesToRemove)
                {
                    // TryResolve<IEngine>()?.UnregisterRenderable(kvp.Value);
                    _sprites.Remove(kvp.Key);
                    _ordered.Remove(kvp.Value);
                }
            }
        }

        public WeakSpriteReference MakeWeakReference(SpriteLease lease, int index)
        {
            lock (_syncRoot)
            {
                if (lease == null)
                    return new WeakSpriteReference(null, null, 0);
                _sprites.TryGetValue(lease.Key, out var entry);
                return new WeakSpriteReference(entry, lease, index);
            }
        }

        /*
        protected override void Subscribed()
        {
            var engine = TryResolve<IEngine>();
            if (engine != null)
                lock (_syncRoot)
                    foreach (var sprite in _sprites)
                        engine.RegisterRenderable(sprite.Value);
            base.Subscribed();
        }
        protected override void Unsubscribed()
        {
            var engine = TryResolve<IEngine>();
            if (engine != null)
                lock (_syncRoot)
                    foreach (var sprite in _sprites)
                        engine.UnregisterRenderable(sprite.Value);
            base.Unsubscribed();
        }
        */
    }
}