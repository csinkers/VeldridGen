using System;
using UAlbion.Core.Veldrid.Events;
using VeldridCodeGen.Interfaces;

namespace UAlbion.Core.SpriteRenderer
{
    [VertexFormat(typeof(Vertex2DTextured))]
    [VertexFormat(typeof(SpriteInstanceData), InstanceStep = 1)]
    [ResourceSet(typeof(SpriteArraySet))]
    [ResourceSet(typeof(CommonSet))]
    public partial class SpriteShader : Component, IDisposable
    {
        public SpriteShader()
        {
            Init();
        }
    }

    // To be generated
    public partial class SpriteShader
    {
        void Init()
        {
            On<DeviceCreatedEvent>(e => Dirty());
        }

        protected override void Subscribed() => Dirty();

        protected override void Unsubscribed() => Dispose();

        void Dirty() => On<PrepareFrameResourceSetsEvent>(Update);

        void Update(IVeldridInitEvent e)
        {
            Off<PrepareFrameResourceSetsEvent>();
        }

        public void Dispose()
        {
        }
    }
}
