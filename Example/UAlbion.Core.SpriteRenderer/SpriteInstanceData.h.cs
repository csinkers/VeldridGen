using System.Runtime.CompilerServices;
using Veldrid;

namespace UAlbion.Core.SpriteRenderer
{
    // To be generated
    public partial struct SpriteInstanceData
    {
        public static readonly uint StructSize = (uint)Unsafe.SizeOf<SpriteInstanceData>();
        public static readonly VertexLayoutDescription Layout = new(
            new VertexElementDescription("iTransform1", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float3),
            new VertexElementDescription("iTransform2", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float3),
            new VertexElementDescription("iTransform3", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float3),
            new VertexElementDescription("iTransform4", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float3),
            new VertexElementDescription("iTexOffset", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
            new VertexElementDescription("iTexSize", VertexElementSemantic.TextureCoordinate, VertexElementFormat.Float2),
            new VertexElementDescription("iTexLayer", VertexElementSemantic.TextureCoordinate, VertexElementFormat.UInt1),
            new VertexElementDescription("iFlags", VertexElementSemantic.TextureCoordinate, VertexElementFormat.UInt1))
        {
            InstanceStepRate = 1
        };
    }
}
