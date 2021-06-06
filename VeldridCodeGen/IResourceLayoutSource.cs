using Veldrid;

namespace UAlbion.Core
{
    public interface IResourceLayoutSource
    {
        ResourceLayout Get(ResourceLayoutDescription description);
    }
}