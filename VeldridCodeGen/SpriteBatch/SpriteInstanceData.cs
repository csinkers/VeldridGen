using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using UAlbion.Api.Visual;
using UAlbion.CodeGen;
using Veldrid;

namespace UAlbion.Core.SpriteBatch
{
    [SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Not comparable")]
    public partial struct SpriteInstanceData : IInstanceFormat
    {
        public static readonly uint StructSize = (uint)Unsafe.SizeOf<SpriteInstanceData>();
        public override string ToString() => $"SID {Position}:{TexLayer} ({Flags & ~SpriteFlags.DebugMask}) Z:{DebugZ}";

        [InputParam("iTransform1")] public Vector3 Transform1 { get; private set; }
        [InputParam("iTransform2")] public Vector3 Transform2 { get; private set; }
        [InputParam("iTransform3")] public Vector3 Transform3 { get; private set; }
        [InputParam("iTransform4")] public Vector3 Transform4 { get; private set; }
        [InputParam("iTexOffset")] public Vector2 TexPosition { get; } // Normalised texture coordinates
        [InputParam("iTexSize")] public Vector2 TexSize { get; } // Normalised texture coordinates
        [InputParam("iTexLayer")] public uint TexLayer { get; }
        [InputParam("iFlags")] public SpriteFlags Flags { get; set; }

        // Derived properties for use by C# code
        public void OffsetBy(Vector3 offset) => Transform4 += offset;

        public Vector3 Position
        {
            get => Transform4;
            set => Transform4 = value;
        }

        public int DebugZ => (int)((1.0f - Transform4.Z) * 4095);

        public Matrix4x4 Transform => new Matrix4x4(
            Transform1.X, Transform1.Y, Transform1.Z, 0,
            Transform2.X, Transform2.Y, Transform2.Z, 0,
            Transform3.X, Transform3.Y, Transform3.Z, 0,
            Transform4.X, Transform4.Y, Transform4.Z, 1);

        // Main constructor
        SpriteInstanceData(Vector3 position, Vector2 size, Region region, SpriteFlags flags)
        {
            if (region == null) throw new ArgumentNullException(nameof(region));
            BuildTransform(position, size, flags, out Matrix4x4 transform);

            Transform1 = new Vector3(transform.M11, transform.M12, transform.M13);
            Transform2 = new Vector3(transform.M21, transform.M22, transform.M23);
            Transform3 = new Vector3(transform.M31, transform.M32, transform.M33);
            Transform4 = new Vector3(transform.M41, transform.M42, transform.M43);
            // Assume right column is always 0,0,0,1

            TexPosition = region.TexOffset;
            TexSize = region.TexSize;
            TexLayer = (uint)region.Layer;
            Flags = flags;
        }

        public void SetTransform(Vector3 position, Vector2 size)
        {
            BuildTransform(position, size, Flags, out var transform);
            Transform1 = new Vector3(transform.M11, transform.M12, transform.M13);
            Transform2 = new Vector3(transform.M21, transform.M22, transform.M23);
            Transform3 = new Vector3(transform.M31, transform.M32, transform.M33);
            Transform4 = new Vector3(transform.M41, transform.M42, transform.M43);
            // Assume right column is always 0,0,0,1
        }

        static void BuildTransform(Vector3 position, Vector2 size, SpriteFlags flags, out Matrix4x4 transform)
        {
            var offset = (flags & SpriteFlags.AlignmentMask) switch
            {
                0                                                   => Vector3.Zero,
                SpriteFlags.MidAligned                              => new Vector3(0, -0.5f, 0),
                SpriteFlags.BottomAligned                           => new Vector3(0, -1.0f, 0),
                SpriteFlags.LeftAligned                             => new Vector3(0.5f, 0, 0),
                SpriteFlags.LeftAligned | SpriteFlags.MidAligned    => new Vector3(0.5f, -0.5f, 0),
                SpriteFlags.LeftAligned | SpriteFlags.BottomAligned => new Vector3(0.5f, -1.0f, 0),
                _ => Vector3.Zero
            };

            transform = Matrix4x4.CreateTranslation(offset);

            if ((flags & SpriteFlags.Floor) != 0)
            {
                transform *= new Matrix4x4(
                    1, 0, 0, 0,
                    0, 0,-1, 0,
                    0, 1, 0, 0,
                    0, 0, 0, 1);
            }

            transform *= Matrix4x4.CreateScale(new Vector3(size.X, size.Y, size.X));
            transform *= Matrix4x4.CreateTranslation(position);
        }

        // Convenience constructors
        public static SpriteInstanceData CopyFlags(Vector3 position, Vector2 size, Region region, SpriteFlags flags) => new SpriteInstanceData(position, size, region, flags);
        public static SpriteInstanceData TopLeft(Vector3 position, Vector2 size, Region region, SpriteFlags flags) => new SpriteInstanceData(position, size, region, flags | SpriteFlags.LeftAligned);
        public static SpriteInstanceData MidLeft(Vector3 position, Vector2 size, Region region, SpriteFlags flags) => new SpriteInstanceData(position, size, region, flags | SpriteFlags.LeftAligned | SpriteFlags.MidAligned);
        public static SpriteInstanceData BottomLeft(Vector3 position, Vector2 size, Region region, SpriteFlags flags) => new SpriteInstanceData(position, size, region, flags | SpriteFlags.LeftAligned | SpriteFlags.BottomAligned);
        public static SpriteInstanceData TopMid(Vector3 position, Vector2 size, Region region, SpriteFlags flags) => new SpriteInstanceData(position, size, region, flags);
        public static SpriteInstanceData Centred(Vector3 position, Vector2 size, Region region, SpriteFlags flags) => new SpriteInstanceData(position, size, region, flags | SpriteFlags.MidAligned);
        public static SpriteInstanceData BottomMid(Vector3 position, Vector2 size, Region region, SpriteFlags flags) => new SpriteInstanceData(position, size, region, flags | SpriteFlags.BottomAligned);

        public static SpriteInstanceData CopyFlags(Vector3 position, Vector2 size, SpriteLease lease, int subImageId, SpriteFlags flags)
        {
            if (lease == null) throw new ArgumentNullException(nameof(lease));
            var subImage = lease.Key.Texture.Regions[subImageId];
            return CopyFlags(position, size, subImage, flags);
        }

        public static SpriteInstanceData TopLeft(Vector3 position, Vector2 size, SpriteLease lease, int subImageId, SpriteFlags flags)
        {
            if (lease == null) throw new ArgumentNullException(nameof(lease));
            var subImage = lease.Key.Texture.Regions[subImageId];
            return TopLeft(position, size, subImage, flags);
        }
        public static SpriteInstanceData MidLeft(Vector3 position, Vector2 size, SpriteLease lease, int subImageId, SpriteFlags flags)
        {
            if (lease == null) throw new ArgumentNullException(nameof(lease));
            var subImage = lease.Key.Texture.Regions[subImageId];
            return MidLeft(position, size, subImage, flags);
        }
        public static SpriteInstanceData BottomLeft(Vector3 position, Vector2 size, SpriteLease lease, int subImageId, SpriteFlags flags)
        {
            if (lease == null) throw new ArgumentNullException(nameof(lease));
            var subImage = lease.Key.Texture.Regions[subImageId];
            return BottomLeft(position, size, subImage, flags);
        }
        public static SpriteInstanceData TopMid(Vector3 position, Vector2 size, SpriteLease lease, int subImageId, SpriteFlags flags)
        {
            if (lease == null) throw new ArgumentNullException(nameof(lease));
            var subImage = lease.Key.Texture.Regions[subImageId];
            return TopMid(position, size, subImage, flags);
        }
        public static SpriteInstanceData Centred(Vector3 position, Vector2 size, SpriteLease lease, int subImageId, SpriteFlags flags)
        {
            if (lease == null) throw new ArgumentNullException(nameof(lease));
            var subImage = lease.Key.Texture.Regions[subImageId];
            return Centred(position, size, subImage, flags);
        }
        public static SpriteInstanceData BottomMid(Vector3 position, Vector2 size, SpriteLease lease, int subImageId, SpriteFlags flags)
        {
            if (lease == null) throw new ArgumentNullException(nameof(lease));
            var subImage = lease.Key.Texture.Regions[subImageId];
            return BottomMid(position, size, subImage, flags);
        }
    }
    // To be generated
    public partial struct SpriteInstanceData
    {
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