using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace VeldridGen
{
    public class GenerationContext
    {
        readonly Action<Diagnostic> _reportFunc;
        public Symbols Symbols { get; }
        public Dictionary<INamedTypeSymbol, VeldridTypeInfo> Types { get; }

        public GenerationContext(Compilation compilation, VeldridSyntaxReceiver receiver, Action<Diagnostic> reportFunc)
        {
            _reportFunc = reportFunc ?? throw new ArgumentNullException(nameof(reportFunc));
            try
            {
                Symbols = new Symbols(compilation);
            }
            catch (TypeResolutionException e)
            {
                reportFunc(Diagnostic.Create(Diag.TypeResolution, null, e.TypeName));
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

        public void Report(DiagnosticSeverity severity, string message) => _reportFunc(Diagnostic.Create(Diag.Generic, null, severity, message));
        public void Report(string message) => _reportFunc(Diagnostic.Create(Diag.Generic, null, message));
        public void Report(string message, Location location) => _reportFunc(Diagnostic.Create(Diag.Generic, location, message));
        public void Report(DiagnosticDescriptor descriptor, Location location, params object[] arguments) => _reportFunc(Diagnostic.Create(descriptor, location, arguments));
    }
}