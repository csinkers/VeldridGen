﻿using UAlbion.Api.Visual;

namespace UAlbion.Core.SpriteBatch
{
    public interface ITextureSource
    {
        Texture2DHolder GetSimpleTexture(ITexture texture);
        Texture2DArrayHolder GetArrayTexture(IArrayTexture texture);
    }
}