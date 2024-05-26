using System;
using System.Collections.Generic;
using VeldridGen.Example.Engine.Events;

namespace VeldridGen.Example.Engine.Visual.Sprites;

public class SpriteManager : ServiceComponent<ISpriteManager>, ISpriteManager
{
    const double CacheCheckIntervalSeconds = 12.0;
    readonly object _syncRoot = new();
    readonly Dictionary<SpriteKey, SpriteBatch> _sprites = new();
    readonly List<SpriteBatch> _batches = new();
    float _lastCleanup;
    float _totalTime;

    public SpriteManager() => On<EngineUpdateEvent>(OnUpdate);

    public SpriteLease Borrow(SpriteKey key, int length, object caller)
    {
        if (length <= 0) throw new ArgumentOutOfRangeException(nameof(length));
        lock (_syncRoot)
        {
            var factory = Resolve<ISpriteFactory>();
            if (!_sprites.TryGetValue(key, out var entry))
            {
                entry = AttachChild(factory.CreateSpriteBatch(key));
                _sprites[key] = entry;
                _batches.Add(entry);
            }

            return entry.Grow(length, caller);
        }
    }

    void OnUpdate(EngineUpdateEvent e)
    {
        _totalTime += (float)e.DeltaSeconds;

        if (_totalTime - _lastCleanup <= CacheCheckIntervalSeconds)
            return;

        lock (_syncRoot)
        {
            var spritesToRemove = new List<KeyValuePair<SpriteKey, SpriteBatch>>();
            foreach (var kvp in _sprites)
                if (kvp.Value.ActiveInstances == 0)
                    spritesToRemove.Add(kvp);

            foreach (var kvp in spritesToRemove)
            {
                _sprites.Remove(kvp.Key);
                _batches.Remove(kvp.Value);
                RemoveChild(kvp.Value);
            }
        }
        _lastCleanup = _totalTime;
    }


    public void Collect(List<IRenderable> renderables)
    {
        if (renderables == null) throw new ArgumentNullException(nameof(renderables));
        lock (_syncRoot)
        {
            foreach (var kvp in _batches)
                renderables.Add(kvp);
        }
    }
}