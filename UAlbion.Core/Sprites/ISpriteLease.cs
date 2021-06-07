using System;
using System.Numerics;
using UAlbion.Api.Visual;

namespace UAlbion.Core.Sprites
{
    public interface ISpriteLease : IDisposable
    {
        void OffsetAll(Vector3 offset);
        void Set(int index, Vector3 position, Vector2 size, Region region, SpriteFlags flags);
    }
}