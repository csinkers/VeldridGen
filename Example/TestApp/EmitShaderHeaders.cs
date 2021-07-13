using System;
using System.IO;
using VeldridGen.Example.SpriteRenderer;

namespace VeldridGen.Example.TestApp
{
    static class ShaderHeaderEmitter
    {
        const string AutoGenMessage = "This file was auto-generated using VeldridGen. It should not be edited by hand.";
        public static int EmitAll(string path)
        {
            if(string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (!Directory.Exists(path))
            {
                Console.WriteLine($"Directory \"{path}\" not found");
                return 1;
            }

            bool success = true;
            success &= Emit(SpriteVertexShader.ShaderSource(), path);
            success &= Emit(SpriteFragmentShader.ShaderSource(), path);
            return success ? 0 : 1;
        }

        static bool Emit((string filename, string glsl) source, string directory)
        {
            var path = Path.Combine(directory, source.filename);
            if (File.Exists(path))
            {
                var currentText = File.ReadAllText(path);
                if (!currentText.Contains(AutoGenMessage))
                {
                    Console.WriteLine($"!! ShaderHeaderEmitter: Tried to overwrite \"{path}\", but the file already exists and does not contain the expected VeldridGen auto-generation message.");
                    return false;
                }
            }

            File.WriteAllText(path, source.glsl);
            Console.WriteLine($"Wrote shader header {path}");
            return true;
        }
    }
}
