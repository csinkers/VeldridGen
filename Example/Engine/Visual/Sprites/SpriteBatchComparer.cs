﻿using System.Collections.Generic;

namespace VeldridGen.Example.Engine.Visual.Sprites;

public class SpriteBatchComparer : IComparer<SpriteBatch>
{
    public static SpriteBatchComparer Instance { get; } = new();
    public int Compare(SpriteBatch x, SpriteBatch y)
    {
        if (ReferenceEquals(x, y)) return 0;
        if (ReferenceEquals(null, y)) return 1;
        if (ReferenceEquals(null, x)) return -1;
        return x.Key.CompareTo(y.Key);
    }
}
