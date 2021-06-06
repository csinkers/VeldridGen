using System;
using Veldrid;

namespace UAlbion.Core.SpriteBatch
{
    public class SpriteRenderer : Component, IRenderer
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
            _vertexBuffer = new MultiBuffer<Vertex2DTextured>(Vertices, BufferUsage.VertexBuffer, "SpriteVertexBuffer");
            _indexBuffer = new MultiBuffer<ushort>(Indices, BufferUsage.IndexBuffer, "SpriteIndexBuffer");
            _pipeline = new Pipeline(
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
            };
        }

        public void Render(MultiSprite batch, CommonSet commonSet, CommandList cl)
        {
            if (batch == null) throw new ArgumentNullException(nameof(batch));

            cl.PushDebugGroup(batch.Name);
            if (batch.Key.ScissorRegion.HasValue)
            {
                var rect = batch.Key.ScissorRegion.Value;
                cl.SetScissorRect(0, (uint)rect.X, (uint)rect.Y, (uint)rect.Width, (uint)rect.Height);
            }

            cl.SetPipeline(_pipeline.DevicePipeline);
            cl.SetGraphicsResourceSet(0, batch.ResourceSet.DeviceSet);
            cl.SetGraphicsResourceSet(1, commonSet.DeviceSet);
            cl.SetVertexBuffer(0, _vertexBuffer.DeviceBuffer);
            cl.SetIndexBuffer(_indexBuffer.DeviceBuffer, IndexFormat.UInt16);
            cl.SetVertexBuffer(1, batch.Instances.DeviceBuffer);

            cl.DrawIndexed((uint)Indices.Length, (uint)batch.ActiveInstances, 0, 0, 0);

            if (batch.Key.ScissorRegion.HasValue)
                cl.SetFullScissorRect(0);
            cl.PopDebugGroup();
        }
    }
}