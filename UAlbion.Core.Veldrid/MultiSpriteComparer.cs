using System.Collections.Generic;

namespace UAlbion.Core.Veldrid
{
    class MultiSpriteComparer : IComparer<SpriteBatch.SpriteBatch>
    {
        public static MultiSpriteComparer Instance { get; } = new();
        public int Compare(SpriteBatch.SpriteBatch x, SpriteBatch.SpriteBatch y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            return x.Key.CompareTo(y.Key);
        }
    }
}