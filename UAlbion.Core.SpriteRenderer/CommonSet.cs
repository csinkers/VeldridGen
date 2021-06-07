using UAlbion.Core.Veldrid;
using Veldrid;
using VeldridCodeGen.Interfaces;

namespace UAlbion.Core.SpriteRenderer
{
    public partial class CommonSet : Component, IResourceLayout
    {
        [Resource("_Shared")]                          SingleBuffer<GlobalInfo>       _globalInfo; 
        [Resource("_Projection", ShaderStages.Vertex)] SingleBuffer<ProjectionMatrix> _projection; 
        [Resource("_View",       ShaderStages.Vertex)] SingleBuffer<ViewMatrix>       _view; 
        [Resource("uPalette",    ShaderStages.Fragment)] Texture2DHolder              _palette;
    }
}
