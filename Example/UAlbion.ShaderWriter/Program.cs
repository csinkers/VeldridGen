using System;
using System.IO;
using UAlbion.Core.SpriteRenderer;

namespace UAlbion.ShaderWriter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: UAlbion.ShaderWriter \"full path of shader directory\"");
                return;
            }

            if (!Directory.Exists(args[0]))
            {
                Console.WriteLine("Usage: UAlbion.ShaderWriter \"full path of shader directory\"");
                Console.WriteLine($"Directory \"{args[0]}\" not found");
                return;
            }

            Emit(SpriteVertexShader.ShaderSource(), args[0]);
            Emit(SpriteFragmentShader.ShaderSource(), args[0]);
        }

        static void Emit((string filename, string glsl) source, string directory)
        {
            var path = Path.Combine(directory, source.filename);
            File.WriteAllText(path, source.glsl);
        }
    }
}
