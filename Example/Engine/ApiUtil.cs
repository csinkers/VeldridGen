using System;
using System.Numerics;
using System.Reflection;

namespace VeldridGen.Example.Engine
{
    public static class ApiUtil
    {
        public static Matrix4x4 Inverse(this Matrix4x4 src)
        {
            Matrix4x4.Invert(src, out Matrix4x4 result);
            return result;
        }

        public static void Assert(string message)
        {
            #if DEBUG
            var oldColour = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Assertion failed! " + message);
            Console.ForegroundColor = oldColour;
            #endif
            CoreTrace.Log.AssertFailed(message);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "Don't care")]
        public static void Assert(bool condition)
        {
            if (!condition)
                Assert("Assertion failed!");
        }

        public static void Assert(bool condition, string message)
        {
            if (!condition)
                Assert($"Assertion failed! {message}");
        }

        public static bool IsFlagsEnum(Type type) => type is {IsEnum: true} && type.GetCustomAttribute(typeof(FlagsAttribute)) != null;

        public static (byte, byte, byte, byte) UnpackColor(uint c)
        {
            var r = (byte)(c & 0xff);
            var g = (byte)((c >> 8) & 0xff);
            var b = (byte)((c >> 16) & 0xff);
            var a = (byte)((c >> 24) & 0xff);
            return (r, g, b, a);
        }

        public static uint PackColor(byte r, byte g, byte b, byte a) =>
            r
            | (uint)(g << 8)
            | (uint)(b << 16)
            | (uint)(a << 24);
    }
}
