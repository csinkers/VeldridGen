using System;
using UAlbion.Api.Visual;

namespace UAlbion.Core.Sprites
{
    public readonly struct SpriteKey : IEquatable<SpriteKey>, IComparable<SpriteKey>
    {
        public SpriteKey(IArrayTexture texture, SpriteSampler sampler, SpriteKeyFlags flags, (int x, int y, int w, int h)? scissorRegion = null)
        {
            Texture = texture ?? throw new ArgumentNullException(nameof(texture));
            Sampler = sampler;
            Flags = flags;
            ScissorRegion = scissorRegion;
        }

        public IArrayTexture Texture { get; }
        public SpriteSampler Sampler { get; }
        public SpriteKeyFlags Flags { get; }
        public (int x, int y, int w, int h)? ScissorRegion { get; }

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
            var samplerComparison = Sampler.CompareTo(other.Sampler);
            if (samplerComparison != 0) return samplerComparison;
            var flagsComparison = Flags.CompareTo(other.Flags);
            if (flagsComparison != 0) return flagsComparison;
            return Nullable.Compare(ScissorRegion, other.ScissorRegion);
        }
    }
}
