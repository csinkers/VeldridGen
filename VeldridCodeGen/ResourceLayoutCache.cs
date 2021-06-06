using System;
using Veldrid;

namespace UAlbion.Core
{
    public class ResourceLayoutCache : Component, IResourceLayoutSource
    {
        public ResourceLayout Get(ResourceLayoutDescription description)
        {
            throw new NotImplementedException();
        }

        // _perSpriteResourceLayout = device.ResourceFactory.CreateResourceLayout(PerSpriteLayoutDescription);
        // _disposeCollector.Add(_perSpriteResourceLayout);
    }
}