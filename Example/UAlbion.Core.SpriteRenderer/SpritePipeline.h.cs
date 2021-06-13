using Veldrid;

namespace UAlbion.Core.SpriteRenderer
{
    public partial class SpritePipeline 
    {
        static VertexLayoutDescription SpriteInstanceDataLayout
        {
            get
            {
                var layout = SpriteInstanceData.Layout;
                layout.InstanceStepRate = 1;
                return layout;
            }
        }

        public SpritePipeline() : base("SpriteSV.vert", "SpriteSF.frag",
            new[] { Vertex2DTextured.Layout, SpriteInstanceDataLayout },
            new[] { typeof(CommonSet), typeof(SpriteArraySet) })
        {
        }
    }
}