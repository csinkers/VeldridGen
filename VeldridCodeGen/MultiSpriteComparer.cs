using System.Collections.Generic;
using UAlbion.Core.SpriteBatch;

namespace UAlbion.Core
{
    class MultiSpriteComparer : IComparer<MultiSprite>
    {
        public static MultiSpriteComparer Instance { get; } = new();
        public int Compare(MultiSprite x, MultiSprite y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            return x.Key.CompareTo(y.Key);
        }
    }
}