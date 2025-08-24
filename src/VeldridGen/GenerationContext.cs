using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VeldridGen;

public class GenerationContext
{
    readonly SourceProductionContext _context;
    public AllSymbols Symbols { get; }
    public Dictionary<INamedTypeSymbol, VeldridTypeInfo> Types { get; }

    public GenerationContext(Compilation compilation, ImmutableArray<TypeDeclarationSyntax> types, SourceProductionContext context)
    {
        _context = context;
        try
        {
            Symbols = new AllSymbols(compilation);
        }
        catch (TypeResolutionException e)
        {
            context.ReportDiagnostic(Diagnostic.Create(Diag.TypeResolution, null, e.TypeName));
            return;
        }

#pragma warning disable RS1024 // Compare symbols correctly
        Types = new Dictionary<INamedTypeSymbol, VeldridTypeInfo>(SymbolEqualityComparer.Default);
#pragma warning restore RS1024 // Compare symbols correctly

        foreach (var type in types)
        {
            var sym = (INamedTypeSymbol)GetSymbol(compilation, type);
            if (sym == null)
                return;

            if (Types.ContainsKey(sym))
                continue;

            Types[sym] = new VeldridTypeInfo(sym, this);

            foreach (var member in type.Members)
                if (member is FieldDeclarationSyntax or PropertyDeclarationSyntax)
                    PopulateMember(compilation, member);

            /*
            foreach (var field in receiver.Fields)
                PopulateMember(compilation, field);

            foreach (var property in receiver.Properties)
                PopulateMember(compilation, property);
            */
        }
    }

    static ISymbol GetSymbol(Compilation compilation, MemberDeclarationSyntax memberDeclarationSyntax)
    {
        var model = compilation.GetSemanticModel(memberDeclarationSyntax.SyntaxTree);
        if (memberDeclarationSyntax is FieldDeclarationSyntax field)
        {
            VariableDeclaratorSyntax variable = field.Declaration.Variables.Single();
            return model.GetDeclaredSymbol(variable);
        }

        return model.GetDeclaredSymbol(memberDeclarationSyntax);
    }

    void PopulateMember(Compilation compilation, MemberDeclarationSyntax memberDeclarationSyntax)
    {
        var sym = GetSymbol(compilation, memberDeclarationSyntax);
        if (sym == null)
            return;

        var typeSym = sym.ContainingType;
        if (!Types.TryGetValue(typeSym, out var typeInfo))
        {
            typeInfo = new VeldridTypeInfo(typeSym, this);
            Types[typeSym] = typeInfo;
        }

        typeInfo.AddMember(sym, this);
    }

    public void Error(string message) => _context.Error(message);
    public void Warn(string message) => _context.Warn(message);
}
