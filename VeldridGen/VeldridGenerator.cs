using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                throw new InvalidOperationException("Could not retrieve syntax receiver for VeldridGenerator");

            var compilation = context.Compilation;
            // var diag = compilation.GetDiagnostics();
            var symbols = new Symbols(compilation);

#pragma warning disable RS1024 // Compare symbols correctly
            var types = new Dictionary<INamedTypeSymbol, VeldridTypeInfo>(SymbolEqualityComparer.Default);
#pragma warning restore RS1024 // Compare symbols correctly

            ISymbol GetSymbol(MemberDeclarationSyntax memberDeclarationSyntax)
            {
                var model = compilation.GetSemanticModel(memberDeclarationSyntax.SyntaxTree);
                if (memberDeclarationSyntax is FieldDeclarationSyntax field)
                {
                    VariableDeclaratorSyntax variable = field.Declaration.Variables.Single();
                    return model.GetDeclaredSymbol(variable);
                }

                return model.GetDeclaredSymbol(memberDeclarationSyntax);
            }

            void PopulateMember(MemberDeclarationSyntax memberDeclarationSyntax)
            {
                var sym = GetSymbol(memberDeclarationSyntax);
                if (sym == null)
                    return;

                var typeSym = sym.ContainingType;
                if (!types.TryGetValue(typeSym, out var typeInfo))
                {
                    typeInfo = new VeldridTypeInfo(typeSym, symbols);
                    types[typeSym] = typeInfo;
                }

                typeInfo.AddMember(sym, symbols);
            }

            foreach (var field in receiver.Fields)
                PopulateMember(field);

            foreach (var property in receiver.Properties)
                PopulateMember(property);

            foreach (var type in receiver.Types)
            {
                var sym = (INamedTypeSymbol)GetSymbol(type);
                if (sym == null)
                    return;

                if (types.TryGetValue(sym, out var typeInfo)) 
                    continue;

                typeInfo = new VeldridTypeInfo(sym, symbols);
                types[sym] = typeInfo;
            }

            foreach (var type in types.Values.Where(x => x.Flags != 0))
            {
                string source = GenerateType(type, symbols, types);
                if (source != null)
                    context.AddSource($"{type.Symbol.Name}_VeldridGen.cs", source);
            }
        }

        static string GenerateType(VeldridTypeInfo type, Symbols symbols, Dictionary<INamedTypeSymbol, VeldridTypeInfo> types)
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
                ResourceSetGenerator.Generate(sb, type, symbols);
            if ((type.Flags & TypeFlags.IsVertexFormat) != 0)
                VertexFormatGenerator.Generate(sb, type);
            if ((type.Flags & TypeFlags.IsFramebufferHolder) != 0)
                FramebufferGenerator.Generate(sb, type);
            if ((type.Flags & TypeFlags.IsPipelineHolder) != 0)
                PipelineGenerator.Generate(sb, type, types);
            if ((type.Flags & TypeFlags.IsShader) != 0)
                ShaderGenerator.Generate(sb, type);

            if (sb.Length == length)
                return null;

            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
