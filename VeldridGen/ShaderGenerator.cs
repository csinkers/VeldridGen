using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    static class ShaderGenerator
    {
        public static void Generate(StringBuilder sb, VeldridTypeInfo shaderType, Dictionary<INamedTypeSymbol, VeldridTypeInfo> types)
        {
            sb.AppendLine($@"
        public static (string, string) ShaderSource()
        {{
            return (""{shaderType.Shader.Filename}"", @""
//!#version 450 // Comments with //! are just for the VS GLSL plugin
//!#extension GL_KHR_vulkan_glsl: enable
");
            ShaderEnumGenerator.EmitEnums(sb, shaderType, types);
            EmitResourceSets(sb, shaderType, types);
            EmitInputs(sb, shaderType, types);
            EmitOutputs(sb, shaderType, types);

            sb.AppendLine(@""");
        }");
        }

        static void EmitResourceSets(StringBuilder sb, VeldridTypeInfo shaderType, Dictionary<INamedTypeSymbol, VeldridTypeInfo> types)
        {
        }

        static void EmitInputs(StringBuilder sb, VeldridTypeInfo shaderType, Dictionary<INamedTypeSymbol, VeldridTypeInfo> types)
        {
        }

        static void EmitOutputs(StringBuilder sb, VeldridTypeInfo shaderType, Dictionary<INamedTypeSymbol, VeldridTypeInfo> types)
        {
        }
    }
}
