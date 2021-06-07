using System;
using System.Collections.Generic;
using UAlbion.Core.Sprites;

namespace UAlbion.Core.Veldrid.SpriteBatch
{
    public class SpriteManager : Component, ISpriteManager
    {
        readonly object _syncRoot = new();
        readonly Dictionary<SpriteKey, SpriteBatch> _sprites = new();
        readonly List<SpriteBatch> _ordered = new();
        readonly IComparer<SpriteBatch> _comparer;

        public SpriteManager(IComparer<SpriteBatch> comparer)
        {
            _comparer = comparer;
        }

        public ISpriteLease Borrow(SpriteKey key, int length, object caller)
        {
            if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));
            lock (_syncRoot)
            {
                if (!_sprites.TryGetValue(key, out var entry))
                {
                    entry = AttachChild(new SpriteBatch(key));
                    // TryResolve<IEngine>()?.RegisterRenderable(entry);
                    _sprites[key] = entry;
                    _ordered.Add(entry);
                }

                return entry.Grow(length, caller);
            }
        }

        public IReadOnlyList<SpriteBatch> Ordered { get { _ordered.Sort(_comparer); return _ordered; } }

        public void Cleanup()
        {
            lock (_syncRoot)
            {
                var spritesToRemove = new List<KeyValuePair<SpriteKey, SpriteBatch>>();
                foreach (var kvp in _sprites)
                    if (kvp.Value.ActiveInstances == 0)
                        spritesToRemove.Add(kvp);

                foreach (var kvp in spritesToRemove)
                {
                    // TryResolve<IEngine>()?.UnregisterRenderable(kvp.Value);
                    _sprites.Remove(kvp.Key);
                    _ordered.Remove(kvp.Value);
                    RemoveChild(kvp.Value);
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