using System;
using System.Numerics;
using UAlbion.Api.Visual;

namespace UAlbion.Core.Sprites
{
    public interface ISpriteLease : IDisposable
    {
        void Set(int index, Vector3 position, Vector2 size, Region region, SpriteFlags flags);
        void OffsetAll(Vector3 offset);
    }
}