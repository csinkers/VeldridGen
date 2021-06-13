using System;
using UAlbion.Core.Veldrid;
using Veldrid;

namespace UAlbion.Core.SpriteRenderer
{
    public class SpriteRenderer : Component
    {
        readonly MultiBuffer<Vertex2DTextured> _vertexBuffer;
        readonly MultiBuffer<ushort> _indexBuffer;
        readonly SpritePipeline _pipeline;

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
            _pipeline = AttachChild(new SpritePipeline
            {
                Name = "P:Sprite",
                AlphaBlend = BlendStateDescription.SingleAlphaBlend,
                CullMode = FaceCullMode.None,
                DepthStencilMode = DepthStencilStateDescription.DepthOnlyLessEqual,
                FillMode = PolygonFillMode.Solid,
                Topology = PrimitiveTopology.TriangleList,
                UseDepthTest = true,
                UseScissorTest = true,
                Winding = FrontFace.Clockwise,
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

            cl.SetPipeline(_pipeline.Pipeline);
            cl.SetGraphicsResourceSet(0, commonSet.ResourceSet);
            cl.SetGraphicsResourceSet(1, batch.SpriteResources.ResourceSet);
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