using System.Numerics;
using System.Runtime.InteropServices;
using UAlbion.Core.Sprites;
using UAlbion.Core.Veldrid;
using VeldridGen.Interfaces;

namespace UAlbion.Core.SpriteRenderer
{
    [VertexShader(typeof(SpriteVertexShader))]
    [FragmentShader(typeof(SpriteFragmentShader))]
    public partial class SpritePipeline : PipelineHolder { }

    [Name("SpriteSV.vert")]
    [Input(0, typeof(Vertex2DTextured))]
    [Input(1, typeof(SpriteInstanceData), InstanceStep = 1)]
    [ResourceSet(0, typeof(CommonSet))]
    [ResourceSet(1, typeof(SpriteArraySet))]
    [Output(0, typeof(SpriteIntermediateData))]
    public partial class SpriteVertexShader : IVertexShader { }

    [Name("SpriteSF.frag")]
    [Input(0, typeof(SpriteIntermediateData))]
    [ResourceSet(0, typeof(CommonSet))]
    [ResourceSet(1, typeof(SpriteArraySet))]
    [Output(0, typeof(ColorOnly))]
    public partial class SpriteFragmentShader : IFragmentShader { }

    public sealed partial class SpriteArraySet : ResourceSetHolder
    {
        [Resource("uSprite")] Texture2DArrayHolder _texture;
        [Resource("uSpriteSampler")] SamplerHolder _sampler;
        [Resource("_Uniform")] SingleBuffer<SpriteUniform> _uniform;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SpriteUniform  : IUniformFormat // Length must be multiple of 16
    {
        [Uniform("uFlags", EnumPrefix = "SKF")] public SpriteKeyFlags Flags { get; set; } // 4 bytes
        [Uniform("uTexSizeW")] public float TextureWidth { get; set; } // 4 bytes
        [Uniform("uTexSizeH")] public float TextureHeight { get; set; } // 4 bytes
        [Uniform("_pad1")] uint Padding { get; set; } // 4 bytes
    }

    public readonly partial struct Vertex2DTextured : IVertexFormat
    {
        [Vertex("vPosition")] public Vector2 Position { get; }
        [Vertex("vTextCoords")] public Vector2 Texture { get; }

        public Vertex2DTextured(float x, float y, float u, float v)
        {
            Position = new Vector2(x, y);
            Texture = new Vector2(u, v);
        }
    }

    public partial struct SpriteIntermediateData : IVertexFormat
    {
        [Vertex("TexPosition")] public Vector2 TexturePosition;
        [Vertex("Layer", Flat = true)] public float TextureLayer;
        [Vertex("Flags", Flat = true, EnumPrefix = "SF")] public SpriteFlags Flags;
        [Vertex("NormCoords")] public Vector2 NormalisedSpriteCoordinates;
        [Vertex("WorldPosition")] public Vector3 WorldPosition;
    }

    public partial struct ColorOnly : IVertexFormat
    {
        [Vertex("Color")] public Vector4 OutputColor;
    }
}
