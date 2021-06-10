using System.Collections.Generic;
using UAlbion.Api.Visual;

namespace UAlbion.Core.Veldrid
{
    public class TextureSource : Component, ITextureSource
    {
        readonly Dictionary<ITexture, Texture2DHolder> _simple = new();
        readonly Dictionary<ITexture, Texture2DArrayHolder> _array = new();
        public Texture2DHolder GetSimpleTexture(ITexture texture)
        {
            if (!_simple.TryGetValue(texture, out var holder))
            {
                holder = AttachChild(new Texture2DHolder(texture));
                _simple[texture] = holder;
            }
            return holder;
        }

        public Texture2DArrayHolder GetArrayTexture(IArrayTexture texture)
        {
            if (!_array.TryGetValue(texture, out var holder))
            {
                holder = AttachChild(new Texture2DArrayHolder(texture));
                _array[texture] = holder;
            }
            return holder;
        }
    }
}