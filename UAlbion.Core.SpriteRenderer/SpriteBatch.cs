using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UAlbion.Api;
using UAlbion.Core.Sprites;
using UAlbion.Core.Veldrid;
using Veldrid;

namespace UAlbion.Core.SpriteRenderer
{
    public class SpriteBatch : Component
    {
        const int MinSize = 4;
        const double GrowthFactor = 1.5;
        const double ShrinkFactor = 0.3;

        readonly object _syncRoot = new();
        readonly List<SpriteLease> _leases = new();

        public SpriteBatch(SpriteKey key)
        {
            Key = key;
            Instances = AttachChild(new MultiBuffer<SpriteInstanceData>(MinSize, BufferUsage.VertexBuffer, $"B_Inst:{Name}"));

            Uniform = AttachChild(new SingleBuffer<SpriteUniform>(new SpriteUniform
            {
                Flags = Key.Flags,
                TextureWidth = key.Texture.Width,
                TextureHeight = key.Texture.Height
            }, BufferUsage.UniformBuffer, $"B_SpriteUniform:{Name}"));
        }

        public string Name => $"Sprite:{Key.Texture.Name}";
        public override string ToString() => $"Multi:{Name} Flags:{Key.Flags} ({ActiveInstances}/{Instances.Count} instances)";
        public SpriteKey Key { get; }
        public int ActiveInstances { get; private set; }
        public MultiBuffer<SpriteInstanceData> Instances { get; }
        public SingleBuffer<SpriteUniform> Uniform { get; }
        public SpriteArraySet ResourceSet { get; private set; }
        protected override void Subscribed()
        {
            var samplerSource = Resolve<ISpriteSamplerSource>();
            ResourceSet = AttachChild(new SpriteArraySet
            {
                Name = $"RS_Sprite:{Key.Texture.Name}",
                Texture = Resolve<ITextureSource>().GetArrayTexture(Key.Texture),
                Sampler = samplerSource.Get(Key.Sampler),
                Uniform = Uniform
            });
        }

        protected override void Unsubscribed()
        {
            RemoveChild(ResourceSet);
        }

        internal SpriteLease Grow(int length, object caller)
        {
            lock (_syncRoot)
            {
                PerfTracker.IncrementFrameCounter("Sprite Borrows");
                int from = ActiveInstances;
                ActiveInstances += length;
                if (ActiveInstances >= Instances.Count)
                {
                    int newSize = Instances.Count;
                    while (newSize <= ActiveInstances)
                        newSize = (int)(newSize * GrowthFactor);

                    Instances.Resize(newSize);
                }

                var lease = new SpriteLease(this, from, ActiveInstances) { Owner = caller };
                _leases.Add(lease);
                VerifyConsistency();
                return lease;
            }
        }

        public void Shrink(SpriteLease leaseToRemove)
        {
            // TODO: Use a more efficient algorithm, e.g. look for equal sized lease at end of list and swap, use linked list for lease list etc
            lock (_syncRoot)
            {
                var buffer = Instances.Borrow();
                VerifyConsistency();
                PerfTracker.IncrementFrameCounter("Sprite Returns");
                bool shifting = false;
                for (int n = 0; n < _leases.Count; n++)
                {
                    if (!shifting && _leases[n].From == leaseToRemove.From)
                    {
                        _leases.RemoveAt(n);
                        ActiveInstances -= leaseToRemove.Length;
                        shifting = true;
                    }

                    if (shifting && n < _leases.Count)
                    {
                        var lease = _leases[n];
                        for (int i = lease.From; i < lease.To; i++)
                            buffer[i - leaseToRemove.Length] = buffer[i];
                        lease.From -= leaseToRemove.Length;
                        lease.To -= leaseToRemove.Length;
                    }
                }
                VerifyConsistency();

                if ((double)ActiveInstances / Instances.Count < ShrinkFactor)
                {
                    int newSize = Instances.Count;
                    while ((double)ActiveInstances / newSize > ShrinkFactor)
                        newSize = (int)(newSize * ShrinkFactor);
                    newSize = Math.Max(newSize, ActiveInstances);
                    newSize = Math.Max(newSize, MinSize);
                    Instances.Resize(newSize);
                }
            }
        }

        [Conditional("DEBUG")]
        void VerifyConsistency()
        {
            if (_leases.Count > 0)
            {
                // Assert that all leases are in order
                foreach (var lease in _leases)
                    ApiUtil.Assert(lease.Length > 0);
                ApiUtil.Assert(_leases[0].From == 0);
                for (int i = 1; i < _leases.Count; i++)
                    ApiUtil.Assert(_leases[i - 1].To == _leases[i].From);
                ApiUtil.Assert(_leases[^1].To == ActiveInstances);
            }
            else ApiUtil.Assert(ActiveInstances == 0);
        }

        public Span<SpriteInstanceData> Lock(SpriteLease lease, ref bool lockWasTaken)
        {
            PerfTracker.IncrementFrameCounter("Sprite Accesses");
            Monitor.Enter(_syncRoot, ref lockWasTaken);
            var instances = Instances.Borrow();
            return instances.Slice(lease.From, lease.Length);
        }

        public void Unlock(SpriteLease _, bool lockWasTaken) // Might need the lease param later if we do more fine grained locking
        {
            if (lockWasTaken)
                Monitor.Exit(_syncRoot);
        }
    }
}

