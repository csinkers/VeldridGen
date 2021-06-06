using System.Numerics;
using System.Runtime.InteropServices;
using UAlbion.CodeGen;

namespace UAlbion.Core
{
#pragma warning disable CA1051 // Do not declare visible instance fields
    [StructLayout(LayoutKind.Sequential)]
    public struct GlobalInfo : IUniformFormat
    {
        [Uniform("uWorldSpacePosition")] public Vector3 WorldSpacePosition;
        [Uniform("_s_padding_1")] readonly uint _padding1;

        [Uniform("uCameraLookDirection")] public Vector3 CameraDirection;
        [Uniform("_s_padding_2")] readonly uint _padding2;

        [Uniform("uResolution")] public Vector2 Resolution;
        [Uniform("uTime")] public float Time;
        [Uniform("uEngineFlags")] public uint EngineFlags;

        [Uniform("uPaletteBlend")] public float PaletteBlend;
        [Uniform("_s_padding_3")] readonly uint _padding3;
        [Uniform("_s_padding_4")] readonly uint _padding4;
        [Uniform("_s_padding_5")] readonly uint _padding5;
    }

#pragma warning restore CA1051 // Do not declare visible instance fields
}
