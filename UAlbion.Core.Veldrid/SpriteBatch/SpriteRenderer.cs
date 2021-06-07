using System;
using UAlbion.Core.Sprites;
using Veldrid;

namespace UAlbion.Core.Veldrid.SpriteBatch
{
    public class SpriteRenderer : Component
    {
        readonly MultiBuffer<Vertex2DTextured> _vertexBuffer;
        readonly MultiBuffer<ushort> _indexBuffer;
        readonly Pipeline _pipeline;

        static readonly ushort[] Indices = { 0, 1, 2, 2, 1, 3 };
        static readonly Vertex2DTextured[] Vertices =
        {
            new(-0.5f, 0.0f, 0.0f, 0.0f), new(0.5f, 0.0f, 1.0f, 0.0f),
            new(-0.5f, 1.0f, 0.0f, 1.0f), new(0.5f, 1.0f, 1.0f, 1.0f),
        };

        public SpriteRenderer()
        {
            _vertexBuffer = AttachChild(new MultiBuffer<Vertex2DTextured>(Vertices, BufferUsage.VertexBuffer, "SpriteVertexBuffer"));
            _indexBuffer = AttachChild(new MultiBuffer<ushort>(Indices, BufferUsage.IndexBuffer, "SpriteIndexBuffer"));
            _pipeline = AttachChild(new Pipeline(
                "SpriteSV.vert",
                "SpriteSF.frag",
                new [] { Vertex2DTextured.Layout, SpriteInstanceData.Layout },
                new[] { typeof(SpriteArraySet), typeof(CommonSet) })
            {
                Name = "P:Sprite",
                UseDepthTest = true,
                UseScissorTest = true,
                DepthStencilMode = DepthStencilStateDescription.DepthOnlyLessEqual,
                CullMode = FaceCullMode.None,
                Topology = PrimitiveTopology.TriangleList,
                FillMode = PolygonFillMode.Solid,
                Winding = FrontFace.Clockwise,
                AlphaBlend = BlendStateDescription.SingleAlphaBlend,
            });
        }

        public void Render(SpriteBatch batch, CommonSet commonSet, FramebufferHolder framebuffer, CommandList cl)
        {
            if (batch == null) throw new ArgumentNullException(nameof(batch));

            cl.PushDebugGroup(batch.Name);
            if (batch.Key.ScissorRegion.HasValue)
            {
                var rect = batch.Key.ScissorRegion.Value;
                cl.SetScissorRect(0, (uint)rect.x, (uint)rect.y, (uint)rect.w, (uint)rect.h);
            }

            cl.SetPipeline(_pipeline.DevicePipeline);
            cl.SetGraphicsResourceSet(0, batch.ResourceSet.DeviceSet);
            cl.SetGraphicsResourceSet(1, commonSet.DeviceSet);
            cl.SetVertexBuffer(0, _vertexBuffer.DeviceBuffer);
            cl.SetIndexBuffer(_indexBuffer.DeviceBuffer, IndexFormat.UInt16);
            cl.SetVertexBuffer(1, batch.Instances.DeviceBuffer);
            cl.SetFramebuffer(framebuffer.Framebuffer);

            cl.DrawIndexed((uint)Indices.Length, (uint)batch.ActiveInstances, 0, 0, 0);

            if (batch.Key.ScissorRegion.HasValue)
                cl.SetFullScissorRect(0);
            cl.PopDebugGroup();
        }
    }
}