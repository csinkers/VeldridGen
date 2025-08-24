using System;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using VeldridGen.Symbols;

namespace VeldridGen;

public abstract class VeldridGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        IncrementalValuesProvider<TypeDeclarationSyntax> types = context.SyntaxProvider.CreateSyntaxProvider(
            predicate: static (node, token) => IsSyntaxNodeInteresting(node),
            transform: static (ctx, token) => GetSemanticTarget(ctx)
        ).Where(static m => m is not null);

        IncrementalValueProvider<(Compilation, ImmutableArray<TypeDeclarationSyntax>)> compilationAndTypes
            = context.CompilationProvider.Combine(types.Collect());

        context.RegisterSourceOutput(compilationAndTypes, (spc, source) => Execute(source.Item1, source.Item2, spc));
    }

    static bool IsSyntaxNodeInteresting(SyntaxNode node) =>
        node switch
        {
            TypeDeclarationSyntax { AttributeLists.Count: > 0 } => true,
            FieldDeclarationSyntax { AttributeLists.Count: > 0 } => true,
            PropertyDeclarationSyntax { AttributeLists.Count: > 0 } => true,
            _ => false
        };

    static TypeDeclarationSyntax GetSemanticTarget(GeneratorSyntaxContext context)
    {
        (TypeDeclarationSyntax typeSyntax, SyntaxList<AttributeListSyntax> attributeLists) =
            context.Node switch
            {
                TypeDeclarationSyntax tds => (tds, tds.AttributeLists),
                FieldDeclarationSyntax fds => (fds.Parent as TypeDeclarationSyntax, fds.AttributeLists),
                PropertyDeclarationSyntax pds => (pds.Parent as TypeDeclarationSyntax, pds.AttributeLists),
                _ => throw new InvalidOperationException("Unexpected node type")
            };

        foreach (AttributeListSyntax attributeListSyntax in attributeLists)
        {
            foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
            {
                ISymbol attributeSymbol = context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol;
                if (attributeSymbol == null)
                    continue;

                INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                string fullName = attributeContainingTypeSymbol.ToDisplayString();

                if (AttributeSymbols.AttributeNames.Contains(fullName))
                    return typeSyntax;
            }
        }

        return null;
    }

    void Execute(Compilation compilation, ImmutableArray<TypeDeclarationSyntax> types, SourceProductionContext context)
    {
        try
        {
            // if (!Debugger.IsAttached) // Uncomment to prompt for debugger during build
            //     Debugger.Launch();

            var genContext = new GenerationContext(compilation, types, context);

            foreach (var type in genContext.Types.Values)
            {
                string source = GenerateType(type, genContext);
                if (source != null)
                    context.AddSource($"{type.Symbol.Name}_VeldridGen.cs", SourceText.From(source, Encoding.UTF8));
            }
        }
        catch (Exception e)
        {
            context.Error(e.ToString());
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
        sb.AppendLine("using Veldrid;");
        sb.AppendLine($"namespace {type.Symbol.ContainingNamespace.ToDisplayString()}");
        sb.AppendLine("{");
        sb.AppendLine($"    {type.Symbol.DeclaredAccessibility.ToString().ToLower()} partial {kword} {type.Symbol.Name}");
        sb.AppendLine("    {");

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
