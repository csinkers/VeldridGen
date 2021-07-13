using Veldrid;
namespace UAlbion.Core.SpriteRenderer
{
    public partial class SpritePipeline
    {
        static VertexLayoutDescription SpriteInstanceDataLayout
        {
            get
            {
                var layout = global::UAlbion.Core.SpriteRenderer.SpriteInstanceData.Layout;
                layout.InstanceStepRate = 1;
                return layout;
            }
        }


        public SpritePipeline() : base("SpriteSV.vert", "SpriteSF.frag",
            new[] { global::UAlbion.Core.SpriteRenderer.Vertex2DTextured.Layout, SpriteInstanceDataLayout},
            new[] { typeof(global::UAlbion.Core.SpriteRenderer.CommonSet), typeof(global::UAlbion.Core.SpriteRenderer.SpriteArraySet) })
        { }
    }
}
