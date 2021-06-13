using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VeldridCodeGen
{
    [Generator]
    public class VeldridGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForPostInitialization(AddInterfaces);
            context.RegisterForSyntaxNotifications(() => new VeldridSyntaxReceiver());
        }

        void AddInterfaces(GeneratorPostInitializationContext context)
        {
            context.AddSource("VeldridGen_Interfaces.cs", Interfaces.Source);
            context.AddSource("VeldridGen_Attributes.cs", Attributes.Source);
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var receiver = (VeldridSyntaxReceiver)context.SyntaxContextReceiver;
            if (receiver == null)
                throw new InvalidOperationException("Could not retrieve syntax receiver for VeldridGenerator");

            var compilation = context.Compilation;
            var diag = compilation.GetDiagnostics();
            var symbols = new Symbols(compilation);
            var types = new Dictionary<INamedTypeSymbol, VeldridTypeInfo>();

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

            foreach (var type in types.Values.Where(x => x.Flags != 0))
            {
                string source = GenerateType(type);
                context.AddSource($"{type.Symbol.Name}_VeldridGen.cs", source);
            }
        }

        static string GenerateType(VeldridTypeInfo type)
        {
            var sb = new StringBuilder();
            sb.AppendLine($@"using Veldrid;
namespace {type.Symbol.ContainingNamespace.ToDisplayString()}
{{
    {type.Symbol.DeclaredAccessibility.ToString().ToLower()} partial class {type.Symbol.Name}
    {{");

            if ((type.Flags & TypeFlags.IsResourceSetHolder) != 0)
                ResourceSetGenerator.Generate(sb, type);
            if ((type.Flags & TypeFlags.IsVertexFormat) != 0)
                VertexFormatGenerator.Generate(sb, type);
            if ((type.Flags & TypeFlags.IsFramebufferHolder) != 0)
                FramebufferGenerator.Generate(sb, type);

            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        void GenerateGlsl()
        {
            /*
            Vertex:
            Resource sets
            Vertex layouts / input params
            */
        }
    }
}
