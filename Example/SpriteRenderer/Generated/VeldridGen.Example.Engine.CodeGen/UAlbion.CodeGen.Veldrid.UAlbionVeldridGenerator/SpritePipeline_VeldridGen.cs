using Veldrid;
namespace VeldridGen.Example.SpriteRenderer
{
    internal partial class SpritePipeline
    {
        static VertexLayoutDescription GpuSpriteInstanceDataLayout
        {
            get
            {
                var layout = global::VeldridGen.Example.SpriteRenderer.GpuSpriteInstanceData.Layout;
                layout.InstanceStepRate = 1;
                return layout;
            }
        }


        public SpritePipeline() : base("SpriteSV.vert", "SpriteSF.frag",
            new[] { global::VeldridGen.Example.SpriteRenderer.Vertex2DTextured.Layout, GpuSpriteInstanceDataLayout},
            new[] { typeof(global::VeldridGen.Example.SpriteRenderer.CommonSet), typeof(global::VeldridGen.Example.SpriteRenderer.SpriteArraySet) })
        { }
    }
}
