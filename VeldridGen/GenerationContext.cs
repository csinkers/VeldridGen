using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VeldridGen;

public class GenerationContext
{
    readonly GeneratorExecutionContext _gec;
    public AllSymbols Symbols { get; }
    public Dictionary<INamedTypeSymbol, VeldridTypeInfo> Types { get; }

    public GenerationContext(GeneratorExecutionContext gec, VeldridSyntaxReceiver receiver)
    {
        _gec = gec;
        var compilation = gec.Compilation;
        try
        {
            Symbols = new AllSymbols(compilation);
        }
        catch (TypeResolutionException e)
        {
            gec.ReportDiagnostic(Diagnostic.Create(Diag.TypeResolution, null, e.TypeName));
            return;
        }

#pragma warning disable RS1024 // Compare symbols correctly
        Types = new Dictionary<INamedTypeSymbol, VeldridTypeInfo>(SymbolEqualityComparer.Default);
#pragma warning restore RS1024 // Compare symbols correctly

        foreach (var field in receiver.Fields)
            PopulateMember(compilation, field);

        foreach (var property in receiver.Properties)
            PopulateMember(compilation, property);

        foreach (var type in receiver.Types)
        {
            var sym = (INamedTypeSymbol)GetSymbol(compilation, type);
            if (sym == null)
                return;

            if (Types.ContainsKey(sym))
                continue;

            Types[sym] = new VeldridTypeInfo(sym, this);
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

    public void Error(string message) => _gec.Error(message);
    public void Warn(string message) => _gec.Warn(message);
}