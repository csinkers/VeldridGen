using System.Collections.Generic;
using UAlbion.Api.Visual;
using UAlbion.Core.SpriteBatch;

namespace UAlbion.Core
{
    class TextureSource : Component, ITextureSource
    {
        readonly Dictionary<ITexture, Texture2DHolder> _simple = new();
        readonly Dictionary<ITexture, Texture2DArrayHolder> _array = new();
        public Texture2DHolder GetSimpleTexture(ITexture texture)
        {
            if (!_simple.TryGetValue(texture, out var holder))
            {
                holder = new Texture2DHolder(texture);
                _simple[texture] = holder;
            }
            return holder;
        }

        public Texture2DArrayHolder GetArrayTexture(IArrayTexture texture)
        {
            if (!_array.TryGetValue(texture, out var holder))
            {
                holder = new Texture2DArrayHolder(texture);
                _array[texture] = holder;
            }
            return holder;
        }
    }
}