using System;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using UAlbion.Api.Visual;
using UAlbion.Core.Sprites;
using VeldridCodeGen.Interfaces;

namespace UAlbion.Core.SpriteRenderer
{
    [SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "Not comparable")]
    public partial struct SpriteInstanceData : IVertexFormat
    {
        public override string ToString() => $"SID {Position}:{TexLayer} ({Flags & ~SpriteFlags.DebugMask})";

        [Vertex("Transform1")] public Vector3 Transform1 { get; private set; }
        [Vertex("Transform2")] public Vector3 Transform2 { get; private set; }
        [Vertex("Transform3")] public Vector3 Transform3 { get; private set; }
        [Vertex("Transform4")] public Vector3 Transform4 { get; private set; }
        [Vertex("TexOffset")] public Vector2 TexPosition { get; } // Normalised texture coordinates
        [Vertex("TexSize")] public Vector2 TexSize { get; } // Normalised texture coordinates
        [Vertex("TexLayer")] public uint TexLayer { get; }
        [Vertex("Flags", EnumPrefix = "SF")] public SpriteFlags Flags { get; set; }

        // Derived properties for use by C# code
        public void OffsetBy(Vector3 offset) => Transform4 += offset;

        public Vector3 Position
        {
            get => Transform4;
            set => Transform4 = value;
        }

        public Matrix4x4 Transform => new(
            Transform1.X, Transform1.Y, Transform1.Z, 0,
            Transform2.X, Transform2.Y, Transform2.Z, 0,
            Transform3.X, Transform3.Y, Transform3.Z, 0,
            Transform4.X, Transform4.Y, Transform4.Z, 1);

        public SpriteInstanceData(Vector3 position, Vector2 size, Region region, SpriteFlags flags)
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
    }
}