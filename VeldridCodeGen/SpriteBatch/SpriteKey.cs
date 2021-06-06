using System;
using UAlbion.Api.Visual;
using Veldrid;

namespace UAlbion.Core.SpriteBatch
{
    public readonly struct SpriteKey : IEquatable<SpriteKey>, IComparable<SpriteKey>
    {
        public SpriteKey(IArrayTexture texture, SamplerHolder sampler, SpriteKeyFlags flags, Rectangle? scissorRegion = null)
        {
            Texture = texture ?? throw new ArgumentNullException(nameof(texture));
            Sampler = sampler ?? throw new ArgumentNullException(nameof(sampler));
            Flags = flags;
            ScissorRegion = scissorRegion;
        }

        public IArrayTexture Texture { get; }
        public SamplerHolder Sampler { get; }
        public SpriteKeyFlags Flags { get; }
        public Rectangle? ScissorRegion { get; } // UI coordinates

        public bool Equals(SpriteKey other) => 
            Equals(Texture, other.Texture) && 
            Equals(Sampler, other.Sampler) && 
            Flags == other.Flags &&
            ScissorRegion == other.ScissorRegion;

        public override bool Equals(object obj) => obj is SpriteKey other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Texture, Sampler, (int)Flags, ScissorRegion);
        public static bool operator ==(SpriteKey a, SpriteKey b) => Equals(a, b);
        public static bool operator !=(SpriteKey a, SpriteKey b) => !(a == b);

        public int CompareTo(SpriteKey other)
        {
            return Flags.CompareTo(other.Flags);
        }
    }
}
