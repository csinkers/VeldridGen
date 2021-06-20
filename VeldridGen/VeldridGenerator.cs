using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    [Generator]
    public class VeldridGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new VeldridSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var receiver = (VeldridSyntaxReceiver)context.SyntaxContextReceiver;
            if (receiver == null)
            {
                context.ReportDiagnostic(Diagnostic.Create(Diag.Generic, null, "Could not retrieve syntax receiver for VeldridGenerator"));
                return;
            }

            try
            {
                var genContext = new GenerationContext(context.Compilation, receiver, context.ReportDiagnostic);
                foreach (var type in genContext.Types.Values.Where(x => x.Flags != 0))
                {
                    string source = GenerateType(type, genContext);
                    if (source != null)
                        context.AddSource($"{type.Symbol.Name}_VeldridGen.cs", source);
                }
            }
            catch (Exception e)
            {
                context.ReportDiagnostic(Diagnostic.Create(Diag.Generic, null, e.ToString()));
            }
        }

        static string GenerateType(VeldridTypeInfo type, GenerationContext context)
        {
            var kword = type.Symbol.IsReferenceType
                ? type.Symbol.IsRecord ? "record" : "class"
                : "struct";
            
            var sb = new StringBuilder();
            sb.AppendLine($@"using Veldrid;
namespace {type.Symbol.ContainingNamespace.ToDisplayString()}
{{
    {type.Symbol.DeclaredAccessibility.ToString().ToLower()} partial {kword} {type.Symbol.Name}
    {{");

            int length = sb.Length;
            if ((type.Flags & TypeFlags.IsResourceSetHolder) != 0)
                ResourceSetGenerator.Generate(sb, type, context);
            if ((type.Flags & TypeFlags.IsVertexFormat) != 0)
                VertexFormatGenerator.Generate(sb, type);
            if ((type.Flags & TypeFlags.IsFramebufferHolder) != 0)
                FramebufferGenerator.Generate(sb, type);
            if ((type.Flags & TypeFlags.IsPipelineHolder) != 0)
                PipelineGenerator.Generate(sb, type, context);
            if ((type.Flags & TypeFlags.IsShader) != 0)
                ShaderGenerator.Generate(sb, type, context);

            if (sb.Length == length) // If none of the type-specific generators emitted anything then we don't need the file.
                return null;

            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
