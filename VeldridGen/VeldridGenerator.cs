using System;
using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;

namespace VeldridGen
{
    public abstract class VeldridGenerator : ISourceGenerator
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
                // if (!Debugger.IsAttached) // Uncomment to prompt for debugger during build
                //     Debugger.Launch();

                var genContext = new GenerationContext(context.Compilation, receiver, context.ReportDiagnostic);
                foreach (var type in genContext.Types.Values)
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

        string GenerateType(VeldridTypeInfo type, GenerationContext context)
        {
            if ((type.Flags & (
                    TypeFlags.IsResourceSetHolder |
                    TypeFlags.IsVertexFormat |
                    TypeFlags.IsFramebufferHolder |
                    TypeFlags.IsPipelineHolder |
                    TypeFlags.IsShader)) == 0)
            {
                return null;
            }

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
                GenerateResourceSet(sb, type, context);
            if ((type.Flags & TypeFlags.IsVertexFormat) != 0)
                GenerateVertexFormat(sb, type, context);
            if ((type.Flags & TypeFlags.IsFramebufferHolder) != 0)
                GenerateFramebuffer(sb, type, context);
            if ((type.Flags & TypeFlags.IsPipelineHolder) != 0)
                GeneratePipeline(sb, type, context);
            if ((type.Flags & TypeFlags.IsShader) != 0)
                GenerateShader(sb, type, context);

            if (sb.Length == length) // If none of the type-specific generators emitted anything then we don't need the file.
                return null;

            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        protected abstract void GenerateResourceSet(StringBuilder sb, VeldridTypeInfo type, GenerationContext context);
        protected abstract void GenerateVertexFormat(StringBuilder sb, VeldridTypeInfo type, GenerationContext context);
        protected abstract void GenerateFramebuffer(StringBuilder sb, VeldridTypeInfo type, GenerationContext context);
        protected abstract void GeneratePipeline(StringBuilder sb, VeldridTypeInfo type, GenerationContext context);
        protected abstract void GenerateShader(StringBuilder sb, VeldridTypeInfo type, GenerationContext context);
    }
}

